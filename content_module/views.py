from datetime import date
from django.shortcuts import render
from django.template import loader

from .models import Moment
from .table_service import AzureTableService

# Create your views here.
def index(request):
    moments = AzureTableService().query_moments('test_moment')
    moment_list = []
    for m in moments:
        moment_list.append(Moment(
            topic=m.PartitionKey,
            record_date=m.RowKey,
            content=m.content))
    
    context = {'moment_list': moment_list, 'timeline_topic': 'test_moment'}
    return render(request, 'index.html', context)

def edit(request):
    return render(request, 'edit.html')