from django import views
from django.shortcuts import render
from django.views.decorators import gzip
from django.http import StreamingHttpResponse
import cv2
import threading
import math
import mediapipe as mp
import socket
from rest_framework.response import Response
from rest_framework import status
from rest_framework.decorators import api_view
from django.http import JsonResponse
from django.views.generic import TemplateView
from PIL import Image 
import json
from . import models
import numpy as np

user_list = {}

def dist(a, b):
    return math.sqrt(
        math.pow(a.x - b.x, 2) + math.pow(a.y - b.y, 2)
    )
    
class LiveVideoFaceDetect(TemplateView):
    template_name = 'video.html'

    def post(self, request, *args, **kwargs):
        return JsonResponse(status=200, data={'message': 'Face detected'})


def processing(request):
    context = {}

    return

class VideoCamera(object):
    def __init__(self):
        self.mp_drawing = mp.solutions.drawing_utils
        self.mp_drawing_styles = mp.solutions.drawing_styles
        self.mp_hands = mp.solutions.hands
        self.video = cv2.VideoCapture(cv2.CAP_DSHOW+0)
        # self.video = cv2.VideoCapture(0)
        (self.grabbed, self.frame1) = self.video.read()
        threading.Thread(target=self.update, args=()).start()

    def __del__(self):
        self.video.release()

    def processing(self):
        with self.mp_hands.Hands(
            model_complexity=0,
            min_detection_confidence=0.5,
            min_tracking_confidence=0.5
        ) as hands:
            # frame 그대로쓰면 update에서 read로 받아오는 프레임때문에 화면 좌우 중복된다.
            self.frame2 = self.frame1
            # 필요에 따라 성능 향상을 위해 이미지 작성을 불가능함으로 기본 설정.
            self.frame2.flags.writeable = False

            # 입력때부터 이미지 뒤집기. OpenCV는 BGR, MediaPipe는 RGB 사용함.
            self.frame2 = cv2.cvtColor(cv2.flip(self.frame2, 1), cv2.COLOR_BGR2RGB)
            results = hands.process(self.frame2)

            # 이미지에 손 주석 그리기.
            self.frame2.flags.writeable = True
            # 처리한 이미지 다시 BGR로
            self.frame2 = cv2.cvtColor(self.frame2, cv2.COLOR_RGB2BGR)
            self.text_f = 'Stop'
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
                        self.text_f = 'Up'
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
                        self.text_f = 'Left'
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
                        self.text_f = 'Right'
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
                        self.text_f = 'Down'
                    else:
                        self.text_f = 'Stop'

                    cv2.putText(
                        self.frame2,
                        text=self.text_f,
                        org=(10, 30),
                        fontFace=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1,
                        color=255, thickness=3
                    )
                    self.mp_drawing.draw_landmarks(
                        self.frame2,
                        hand_landmarks,
                        self.mp_hands.HAND_CONNECTIONS,
                        self.mp_drawing_styles.get_default_hand_landmarks_style(),
                        self.mp_drawing_styles.get_default_hand_connections_style()
                    )
                
    def get_frame(self):
        self.processing()
        # 인식, 반전처리된 프레임만 전송.
        image = self.frame2
        _, jpeg = cv2.imencode('.jpg', image)
        return jpeg.tobytes()

    def update(self):
        while True:
            (self.grabbed, self.frame1) = self.video.read()   

def ImageProcessing(input_image):
    mp_drawing = mp.solutions.drawing_utils
    mp_drawing_styles = mp.solutions.drawing_styles
    mp_hands = mp.solutions.hands
    frame1 = input_image
    with mp_hands.Hands(
        static_image_mode=True,
        model_complexity=0,
        min_detection_confidence=0.5,
        min_tracking_confidence=0.5
        # static_image_mode=True,
        # max_num_hands=2,
        # min_detection_confidence=0.5
    ) as hands:
        # frame 그대로쓰면 update에서 read로 받아오는 프레임때문에 화면 좌우 중복된다.
        frame2 = frame1
        # 필요에 따라 성능 향상을 위해 이미지 작성을 불가능함으로 기본 설정.
        frame2.flags.writeable = False
        # cv2.imshow('ata', frame2)
        # cv2.waitKey(1000)

        # 입력때부터 이미지 뒤집기. OpenCV는 BGR, MediaPipe는 RGB 사용함.
        # frame2 = cv2.cvtColor(cv2.flip(frame2, 1), cv2.COLOR_BGR2RGB)
        # ndarray로 변환하면서 이미 BGR2RGB처리됨.
        frame2 = cv2.flip(frame2, 1)
        results = hands.process(frame2)

        # 이미지에 손 주석 그리기.
        frame2.flags.writeable = True
        # 처리한 이미지 다시 BGR로
        frame2 = cv2.cvtColor(frame2, cv2.COLOR_RGB2BGR)
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
                    frame2,
                    text=text_f,
                    org=(10, 30),
                    fontFace=cv2.FONT_HERSHEY_SIMPLEX, fontScale=1,
                    color=255, thickness=3
                )
                mp_drawing.draw_landmarks(
                    frame2,
                    hand_landmarks,
                    mp_hands.HAND_CONNECTIONS,
                    mp_drawing_styles.get_default_hand_landmarks_style(),
                    mp_drawing_styles.get_default_hand_connections_style()
                )
                
    # 인식, 반전처리된 프레임만 전송.
    image = frame2
    # cv2.imshow('test', image)
    # cv2.waitKey(1000)
    _, jpeg = cv2.imencode('.jpg', image)
    # return jpeg.tobytes()
    return text_f


def gen(camera):
    while True:
        frame = camera.get_frame()
        # client_socket.sendall(camera.text_f.encode())
        yield(b'--frame\r\n'
              b'Content-Type: image/jpeg\r\n\r\n' + frame + b'\r\n\r\n')


@gzip.gzip_page
def detect(request, user_name):
    try:
        # global client_socket, addr
        # HOST = '127.0.0.1'
        # PORT = 8001
        # server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        # server_socket.bind((HOST, PORT))
        # server_socket.listen()
        # client_socket, addr = server_socket.accept()
        # print('Connected by', addr)
        cam = VideoCamera()
        user_list[f'{user_name}'] = cam
        print(user_list)
        return StreamingHttpResponse(gen(cam), content_type="multipart/x-mixed-replace;boundary=frame")
    except:  # This is bad! replace it with proper handling
        print("에러입니다...")
        pass

@api_view(['GET'])
def getControl(request, user_name):
    context = {
        'control': user_list[f'{user_name}'].text_f
    }
    return Response(context, status=status.HTTP_200_OK)

@api_view(['POST'])
def delUserControl(request, user_name):
    del user_list[f'{user_name}']
    return Response(status=status.HTTP_200_OK)

@api_view(['PUT'])
def getBlob(request):
    try:
        print(request.files['file'])
    except:
        try:
            file = request.files['file']
            img = Image.open(request.POST["file"].read())
            img = Image.open(file.stream)
            img.show()
        except:
            pass
    data = json.loads(request.body.decode('utf-8'))
    print(data['data'])
    print(data['data'].read())
    # file = request.files['file']
    # img = Image.open(request.POST["file"].read())
    print('blob')
    # img = Image.open(file.stream)
    # img.show()
    return Response(status=status.HTTP_200_OK)


@api_view(['POST'])
def uploadFile(request):
    print("request",request)
    try:
        print("request.data: ",request.data)
        img = Image.open(request.data['image'])
        # img.show()
        img = np.asarray(img)
        # cv2.imshow('ata', img)
        # cv2.waitKey(1000)
        context = { 
            'control': ImageProcessing(img)
        }
        return Response(context, status=status.HTTP_200_OK)
    except Exception as err:
        print(f'에러: {err}')
    print("request.data의 타입: ",type(request.data))
    print("request.FILES: ",request.FILES)
    print("request.FILES의 타입: ",type(request.FILES))
    print("request.method: "+str(request.method))
    print("request.content_type: ",request.content_type)
    print("request.stream: ",request.stream)
    # print(request.read())
    # with open('file_name.txt', 'wb') as output:
    #     output.write(request.read())
    # context = {
    #     'control': request.read()
    # }
    # return Response(context, status=status.HTTP_200_OK)
    return Response(status=status.HTTP_200_OK)
    # print(request.POST)
    # print(request.FILES)
    # if request.method == "POST":
    #     # Fetching the form data
    #     fileTitle = request.POST["fileTitle"]
    #     # fileTitle = request.POST["blob"]
    #     uploadedFile = request.FILES["uploadedFile"]

    #     # Saving the information in the database
    #     document = models.Document(
    #         title = fileTitle,
    #         uploadedFile = uploadedFile
    #     )
    #     document.save()

    # documents = models.Document.objects.all()

    # return render(request, "Core/upload-file.html", context = {
    #     "files": documents
    # })