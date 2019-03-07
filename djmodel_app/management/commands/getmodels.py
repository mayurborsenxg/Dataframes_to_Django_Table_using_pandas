from django.core.management import BaseCommand
from djmodel import get_model_repr
import pandas as pd

csv = 'http://opendata.praha.eu/dataset/8dbb0d35-692c-4668-b225-c0702853c28e/resource/c4f3157a-ef07-423a-9051-7c748484d6df/download/5d88f52c-78ee-4de3-ad18-9b844737cd63-parking.csv'
    

class Command(BaseCommand):
       
    def handle(self, *args, **options):
        url = input('Enter URL: ')
        if url == '':
            url = csv
        modelname = input('Enter name of class: ').title()
        df = pd.read_csv(url)
        text = get_model_repr(df,col_casing='camel',indent=4,model_name=modelname)
        file = open('djmodel_app/models.py','a')
        file.write(str(text))
        file.close()

