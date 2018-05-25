from django import forms

class MomentForm(forms.Form):
    record_date = forms.DateField(label='Date')
    content = forms.CharField(label='Content', max_length=4000)