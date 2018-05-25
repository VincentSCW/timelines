from datetime import date
from azure.cosmosdb.table.tableservice import TableService
from azure.cosmosdb.table.models import Entity

class AzureTableService(object):
    moment_table_name = 'moment'

    def __init__(self):
        self.__table_service = TableService(account_name='timelines', 
            account_key='1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==',
            connection_string='DefaultEndpointsProtocol=https;AccountName=timelines;AccountKey=1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==;EndpointSuffix=core.chinacloudapi.cn')

    def create_table_if_not_exists(self, table_name):
        if self.__table_service.exists(table_name):
            print('table already exists')
        else:
            self.__table_service.create_table(table_name)
            print('table created')
    
    def add_moment(self, topic, record_date, content):
        if not isinstance(record_date, date):
            raise TypeError('record_date should be date type')

        self.create_table_if_not_exists(AzureTableService.moment_table_name)
        moment = Entity()
        moment.PartitionKey = topic
        moment.RowKey = record_date.strftime('%Y-%m-%d')
        moment.content = content
        self.__table_service.insert_entity(AzureTableService.moment_table_name, moment)
    
    def query_moments(self, topic):
        moments = self.__table_service.query_entities(AzureTableService.moment_table_name,
            filter="PartitionKey eq '%s'" % (topic,))
        return moments
        # for m in moments:
        #     print(m.PartitionKey)
        #     print(m.RowKey)
        #     print(m.content)
