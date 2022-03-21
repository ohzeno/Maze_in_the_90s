import cv2
import mediapipe as mp
import math
import socket
import time

HOST = '127.0.0.1'
PORT = 8000
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
server_socket.bind((HOST, PORT))
server_socket.listen()
client_socket, addr = server_socket.accept()
print('Connected by', addr)

def dist(a, b):
    return math.sqrt(
        math.pow(a.x - b.x, 2) + math.pow(a.y - b.y, 2)
    )


mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_hands = mp.solutions.hands

# 웹캠, 영상 파일의 경우 이것을 사용하세요.:
cap = cv2.VideoCapture(0)
with mp_hands.Hands(
    model_complexity=0,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5
) as hands:
    while cap.isOpened():
        success, image = cap.read()
        if not success:
            print("카메라를 찾을 수 없습니다.")
            # 동영상을 불러올 경우는 'continue' 대신 'break'를 사용.
            continue

        # 필요에 따라 성능 향상을 위해 이미지 작성을 불가능함으로 기본 설정.
        image.flags.writeable = False

        # 입력때부터 이미지 뒤집기. OpenCV는 BGR, MediaPipe는 RGB 사용함.
        image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
        results = hands.process(image)

        # 이미지에 손 주석을 그리기.
        image.flags.writeable = True
        # 처리한 이미지 다시 BGR로
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
        text_f = 'Stop'
        # x는 유저 시점 왼쪽 0, y는 위 0. z는 카메라에서 멀수록 0(가까우면 마이너스)
        if results.multi_hand_landmarks:
            for hand_landmarks in results.multi_hand_landmarks:
                if (
                        (
                                abs(
                                    math.degrees(
                                        math.atan(
                                            (hand_landmarks.landmark[6].y - hand_landmarks.landmark[8].y) /
                                            (hand_landmarks.landmark[6].x - hand_landmarks.landmark[8].x)
                                        )
                                    )
                                ) > 55
                        ) and
                        (hand_landmarks.landmark[8].y < hand_landmarks.landmark[7].y) and
                        (dist(hand_landmarks.landmark[8], hand_landmarks.landmark[5]) /
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[0]) > 0.7) and
                        (dist(hand_landmarks.landmark[5], hand_landmarks.landmark[8]) >
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[17]))
                ):
                    text_f = 'Up'
                elif (
                        (
                                abs(
                                    math.degrees(
                                        math.atan(
                                            (hand_landmarks.landmark[6].y - hand_landmarks.landmark[8].y) /
                                            (hand_landmarks.landmark[6].x - hand_landmarks.landmark[8].x)
                                        )
                                    )
                                ) < 50
                        ) and
                        (hand_landmarks.landmark[8].x < hand_landmarks.landmark[7].x) and
                        (dist(hand_landmarks.landmark[8], hand_landmarks.landmark[5]) /
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[0]) > 0.7) and
                        (dist(hand_landmarks.landmark[5], hand_landmarks.landmark[8]) >
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[17]))
                ):
                    text_f = 'Left'
                elif (
                        (
                                abs(
                                    math.degrees(
                                        math.atan(
                                            (hand_landmarks.landmark[6].y - hand_landmarks.landmark[8].y) /
                                            (hand_landmarks.landmark[6].x - hand_landmarks.landmark[8].x)
                                        )
                                    )
                                ) < 50
                        ) and
                        (hand_landmarks.landmark[8].x > hand_landmarks.landmark[7].x) and
                        (dist(hand_landmarks.landmark[8], hand_landmarks.landmark[5]) /
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[0]) > 0.6) and
                        (dist(hand_landmarks.landmark[5], hand_landmarks.landmark[8]) >
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[17]) * 0.8)
                ):
                    text_f = 'Right'
                elif (
                        (
                                abs(
                                    math.degrees(
                                        math.atan(
                                            (hand_landmarks.landmark[6].y - hand_landmarks.landmark[8].y) /
                                            (hand_landmarks.landmark[6].x - hand_landmarks.landmark[8].x)
                                        )
                                    )
                                ) > 55
                        ) and
                        (hand_landmarks.landmark[8].y > hand_landmarks.landmark[7].y) and
                        (dist(hand_landmarks.landmark[8], hand_landmarks.landmark[5]) /
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[0]) > 0.8) and
                        (dist(hand_landmarks.landmark[5], hand_landmarks.landmark[8]) >
                         dist(hand_landmarks.landmark[5], hand_landmarks.landmark[17]) * 0.9)
                ):
                    text_f = 'Down'
                else:
                    text_f = 'Stop'
                cv2.putText(
                    image,
                    text=text_f,
                    org=(10, 30),
                    fontFace=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1,
                    color=255, thickness=3
                )
                mp_drawing.draw_landmarks(
                    image,
                    hand_landmarks,
                    mp_hands.HAND_CONNECTIONS,
                    mp_drawing_styles.get_default_hand_landmarks_style(),
                    mp_drawing_styles.get_default_hand_connections_style()
                )
        cv2.imshow('MediaPipe Hands', image)
        client_socket.sendall(text_f.encode())
        print(f'{text_f:5} send 완료')
        if cv2.waitKey(5) & 0xFF == 27:
            break
cap.release()
client_socket.close()
server_socket.close()
