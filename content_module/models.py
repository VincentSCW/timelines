from django.db import models

# Create your models here.
class Moment(models.Model):
    topic = models.CharField(max_length=200)
    record_date = models.DateField('date recorded')
    content = models.CharField(max_length=4000)