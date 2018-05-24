from datetime import date

class Moment(object):

    @property
    def PartitionKey(self):
        return self._partition_key
    
    @PartitionKey.setter
    def PartitionKey(self, value):
        self._partition_key = value

    @property
    def RowKey(self):
        return self._row_key
    
    @RowKey.setter
    def RowKey(self, value):
        self._row_key = value

    @property
    def record_date(self):
        return self._date

    @record_date.setter
    def record_date(self, value):
        if not isinstance(value, date):
            raise ValueError('date must be date type')
        self._date = value
    
    @property
    def content(self):
        return self._content
    
    @content.setter
    def content(self, value):
        self._content = value