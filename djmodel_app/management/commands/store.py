from django.core.management import BaseCommand
from djmodel_app.models import Park
from django.db.models import Count, Max
import pandas as pd
from . getmodels import csv

class Command(BaseCommand):

    def handle(self, *args, **options):
        all_objects = Park.objects.all()
        
        try:
            df = pd.read_csv(csv)
            df_list = []
            for i in range(len(df)):
                df_list.append(Park(name=df.name[i],lat=df.lat[i],lng=df.lng[i],pr=df.pr[i],totalNumOfPlaces=df.totalNumOfPlaces[i]))
                df_list[i].save()        

            unique_fields = ['name', 'lat']

            duplicates = (
                Park.objects.values(*unique_fields)
                .order_by()
                .annotate(max_id=Max('id'), count_id=Count('id'))
                .filter(count_id__gt=1)
            )

            for duplicate in duplicates:
                (
                    Park.objects
                    .filter(**{x: duplicate[x] for x in unique_fields})
                    .exclude(id=duplicate['max_id'])
                    .delete()
                )        

        except FileNotFoundError as e:
            print("Enter correct URL")    
                    
        #for i in range(len(all_objects))[len(all_objects)::-1]:
            #obj_list.append(all_obj)