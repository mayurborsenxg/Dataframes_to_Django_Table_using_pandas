from django.contrib import admin
from . models import Park

# Register your models here.
class ParkAdmin(admin.ModelAdmin):
    list_display=['name','lat','lng','pr','totalNumOfPlaces']

admin.site.register(Park,ParkAdmin)