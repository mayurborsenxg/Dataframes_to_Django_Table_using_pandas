from django.shortcuts import render
from djmodel_app.models import Park,ParkTable
from django.http import HttpResponse, HttpResponseRedirect
from django.template import loader
from django_pandas.io import read_frame
from djmodel_app.models import Park

# Create your views here.


def index(request):
    qs = Park.objects.all()
    #print(qs)
    l=[]
    for q in qs:
        l.append({'name':q.name,'lat':q.lat,'lng':q.lng,'pr':q.pr,'totalNumOfPlaces':q.totalNumOfPlaces})
    #table = ParkTable(qs)
    template_name = 'djmodel_app/table.html'
    return render(request,template_name,{'table':l})

'''    
    
    l=[]

    for i in range(len(qs)):
        l.append(qs[i])
    print(qs)
'''    

'''
    df= qs.to_dataframe()
    print(df.head())

    df1 = df.to_html()
    context_name = {'object_list' : df1}
    print(df1)
    return render (request, template_name, context_name)
'''

'''
    qs = Park.objects.all()
    l=[]
    template = loader.get_template('djmodel_app/index.html')
    context = { 
        'QuerySet': qs,
    }
    return HttpResponse(template.render(context, request))
    for i in range(len(qs)):
        
        l.append(qs[i])
    #for q in qs:
        #return HttpResponse(q)
        return HttpResponse(l)
    #html = .to_html
        

'''