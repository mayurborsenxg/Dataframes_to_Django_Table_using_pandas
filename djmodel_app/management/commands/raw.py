from django.core.management import BaseCommand
from ... models import Park
class Command(BaseCommand):

    def handle(self, *args, **options):
        name