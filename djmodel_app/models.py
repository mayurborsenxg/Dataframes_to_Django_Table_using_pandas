from django.db import models
from django_pandas.managers import DataFrameManager
import django_tables2 as tables
from django_tables2 import SingleTableView

# Create your models here.

class Park(models.Model):
    name = models.CharField(max_length=40) # max length was 24
    lat = models.FloatField() # min: 49.986686999999996, max: 50.126156, mean: 50.07130665217392
    lng = models.FloatField() # min: 14.28977, max: 14.597429, mean: 14.461379999999997
    pr = models.BooleanField() # 
    totalNumOfPlaces = models.PositiveSmallIntegerField() # min: 33, max: 1205, mean: 275.9130434782609

    objects = DataFrameManager()

    def __str__(self):
        return self.name+' - '+str(self.lat)+' - '+str(self.lng)+' - '+str(self.pr)+' - '+str(self.totalNumOfPlaces)+'\n'

class ParkTable(tables.Table):
    class Meta:
        model = Park
        exclude = ('id',)

class ParkList(SingleTableView):
    model = Park
    table_class = ParkTable
    
