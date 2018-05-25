from rest_framework.views import APIView
from rest_framework.decorators import api_view
from django.http import JsonResponse

@api_view(['POST'])
def create_moment(request):
    pass