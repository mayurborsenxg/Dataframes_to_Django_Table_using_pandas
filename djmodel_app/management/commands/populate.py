from django.core.management import BaseCommand
from ... models import Park
import pandas as pd

class Command(BaseCommand):

    def handle(self, *args, **options):
        #df = pd.read_csv('pandas_djmodels/park.csv')
        agencies = pd.DataFrame({"name":["Agency 1", "Aggency 2", "Agency 3"]})
        for agency in agencies.itertuples():
            agency = Park.objects.create(name=agency.name)
'''
        from django.conf import settings
        database_name = settings.DATABASES['default']['NAME']
        database_url = 'sqlite:///{}'.format(database_name)
        engine = create_engine(database_url, echo=False)
'''
        #insert data data
        
        #agencies.to_sql("agency",con=engine,if_exists="replace")