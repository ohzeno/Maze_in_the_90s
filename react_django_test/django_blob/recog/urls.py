from django.urls import path
from . import views
from django.conf import settings
from django.conf.urls.static import static

app_name = 'recog'

urlpatterns = [
    path('', views.processing),
    path("upload/<str:user_name>/", views.uploadFile, name = "uploadFile"),
    path('detect/<str:user_name>/', views.detect, name="detect"),
    path('detect/<str:user_name>/control', views.getControl, name = "getControl"),
    path('detect/<str:user_name>/delete', views.getControl, name = "getControl"),
]

if settings.DEBUG: 
    urlpatterns += static(
        settings.MEDIA_URL, 
        document_root = settings.MEDIA_ROOT
    )