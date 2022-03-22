from django.urls import path
from . import views

app_name = 'recog'

urlpatterns = [
    path('', views.processing),
    path('detect/<str:user_name>/', views.detect, name="detect"),
    path('detect/<str:user_name>/control', views.getControl, name = "getControl"),
    path('detect/<str:user_name>/delete', views.getControl, name = "getControl"),
]