from django.urls import path
from django.conf.urls import url, include
from rest_framework import routers, serializers, viewsets
from content_module import views

urlpatterns = [
    path('', views.index),
    path('edit', views.edit)
]
