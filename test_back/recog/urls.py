from django.urls import path
from . import views

app_name = 'recog'

urlpatterns = [
    path('', views.processing),
    path('detectme', views.detectme, name="detectme"),
]