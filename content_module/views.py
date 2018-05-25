from datetime import date
from django.shortcuts import render
from content_module.azure_integration.table_service import AzureTableService

# Create your views here.
def index(request):
    table_service = AzureTableService()
    table_service.query_moments('test_moment')
    return render(request, 'index.html')