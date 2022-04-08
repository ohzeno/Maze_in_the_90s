import cv2
import mediapipe as mp
import math
from PIL import Image


def dist(a, b):
    return math.sqrt(
        math.pow(a.x - b.x, 2) + math.pow(a.y - b.y, 2)
    )

mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_pose = mp.solutions.pose

def get_degree(data, a, b):
    return math.degrees(
        math.atan(
            (data[a].y - data[b].y) / (data[a].x - data[b].x)
        )
    )

# For static images:
# IMAGE_FILES = []
# BG_COLOR = (192, 192, 192) # gray
# with mp_pose.Pose(
#     static_image_mode=True,
#     model_complexity=2,
#     enable_segmentation=True,
#     min_detection_confidence=0.5) as pose:
#   for idx, file in enumerate(IMAGE_FILES):
#     image = cv2.imread(file)
#     image_height, image_width, _ = image.shape
#     # Convert the BGR image to RGB before processing.
#     results = pose.process(cv2.cvtColor(image, cv2.COLOR_BGR2RGB))
#
#     if not results.pose_landmarks:
#       continue
#     print(
#         f'Nose coordinates: ('
#         f'{results.pose_landmarks.landmark[mp_pose.PoseLandmark.NOSE].x * image_width}, '
#         f'{results.pose_landmarks.landmark[mp_pose.PoseLandmark.NOSE].y * image_height})'
#     )
#
#     annotated_image = image.copy()
#     # Draw segmentation on the image.
#     # To improve segmentation around boundaries, consider applying a joint
#     # bilateral filter to "results.segmentation_mask" with "image".
#     condition = np.stack((results.segmentation_mask,) * 3, axis=-1) > 0.1
#     bg_image = np.zeros(image.shape, dtype=np.uint8)
#     bg_image[:] = BG_COLOR
#     annotated_image = np.where(condition, annotated_image, bg_image)
#     # Draw pose landmarks on the image.
#     mp_drawing.draw_landmarks(
#         annotated_image,
#         results.pose_landmarks,
#         mp_pose.POSE_CONNECTIONS,
#         landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style())
#     cv2.imwrite('/tmp/annotated_image' + str(idx) + '.png', annotated_image)
#     # Plot pose world landmarks.
#     mp_drawing.plot_landmarks(
#         results.pose_world_landmarks, mp_pose.POSE_CONNECTIONS)

# For webcam input:
cap = cv2.VideoCapture(0)
with mp_pose.Pose(
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5
) as pose:
    while cap.isOpened():
        success, image = cap.read()
        if not success:
            print("Ignoring empty camera frame.")
            # If loading a video, use 'break' instead of 'continue'.
            continue

        # To improve performance, optionally mark the image as not writeable to
        # pass by reference.
        image.flags.writeable = False

        # 입력때부터 이미지 뒤집기. OpenCV는 BGR, MediaPipe는 RGB 사용함.
        image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)
        results = pose.process(image)
        # print(results)
        # Draw the pose annotation on the image.
        image.flags.writeable = True
        # 처리한 이미지 다시 BGR로
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
        text_f = 'Stop'
        head = results.pose_landmarks.landmark
        # z = round(head[1].z, 2)
        # y = round(head[1].y, 2)
        # x = round(head[1].x, 2)
        aa = round(math.degrees(
                math.atan(
                    (head[8].y - head[4].y)/(head[8].x - head[4].x)
                )
            ), 2)
        bb = round(math.degrees(
                math.atan(
                    (head[7].y - head[1].y)/(head[7].x - head[1].x)
                )
            ), 2)
        cc = round(math.degrees(
            math.atan(
                (head[6].y - head[3].y) / (head[6].x - head[3].x)
            )
        ), 2)
        # text_f = f'x: {x}, y: {y}, z: {z}'
        text_f = f'48: {aa}, 17: {bb}, 63: {cc}'
        # text_f = f'5: {round(head[5].z, 2)}, 0: {round(head[0].z, 2)}, 2: {round(head[2].z,2 )}'
        # x는 유저 시점 왼쪽 0, y는 위 0. z는 카메라에서 멀수록 0(가까우면 마이너스)
        if (
                (abs(head[1].y - head[0].y) * 0.6 <= abs(head[7].y - head[1].y)) and
                (abs(head[4].y - head[0].y) * 0.6 <= abs(head[8].y - head[4].y)) and
                (head[1].y < head[7].y) and (head[4].y < head[8].y) and
                # get_degree(head, 8, 4) > (-65) and
                # get_degree(head, 7, 1) > (25) and
                abs(get_degree(head, 6, 3)) <= 20 and
                abs(dist(head[4], head[8]) - dist(head[1], head[7])) < dist(head[10], head[9]) * 1.2
        ):
            text_f = "Up " + text_f
        elif (
                ((
                        (head[7].y <= head[2].y * 1.003) and
                        (head[8].y <= head[5].y * 1.003)
                ) or
                (
                        (head[8].y <= head[4].y) and (head[7].y <= head[1].y)
                )) and
                abs(dist(head[4], head[8]) - dist(head[1], head[7])) < dist(head[10], head[9])
        ):
            text_f = "Down" + text_f
        elif (
                (get_degree(head, 8, 4) < (-34)) and
                (30 > get_degree(head, 7, 1) > (-20)) and
                (head[5].y > head[2].y) and
                dist(head[6], head[8]) < dist(head[4], head[0]) * 0.9 and
                dist(head[4], head[8]) > dist(head[1], head[7]) * 0.4 and
                get_degree(head, 6, 3) < (-20)
        ):
            text_f = "Left" + text_f
        elif (
                (get_degree(head, 8, 4) > (-15)) and
                (get_degree(head, 7, 1) > (40)) and
                (head[5].y < head[2].y) and
                dist(head[3], head[7]) < dist(head[1], head[0]) * 0.9 and
                dist(head[1], head[7]) > dist(head[4], head[8]) * 0.4 and
                get_degree(head, 6, 3) >= 26
        ):
            text_f = "Right" + text_f
        else:
            text_f = "Stop" + text_f

        cv2.putText(
            image,
            text=text_f,
            org=(10, 30),
            fontFace=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1,
            color=255, thickness=3
        )
        mp_drawing.draw_landmarks(
            image,
            results.pose_landmarks,
            mp_pose.POSE_CONNECTIONS,
            landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style()
        )
        # Flip the image horizontally for a selfie-view display.
        cv2.imshow('MediaPipe Pose', image)
        if cv2.waitKey(5) & 0xFF == 27:
            break
cap.release()
