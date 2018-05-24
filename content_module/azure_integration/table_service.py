from azure.cosmosdb.table.tableservice import TableService
from azure.cosmosdb.table.models import Entity
from moment_entity import Moment

class AzureTableService(object):
    def init_table_service(self):
        return TableService(account_name='timelines', 
            account_key='1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==',
            connection_string='DefaultEndpointsProtocol=https;AccountName=timelines;AccountKey=1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==;EndpointSuffix=core.chinacloudapi.cn')

    def create_table_if_not_exists(self, table_name):
        table_service = self.init_table_service()
        if table_service.exists(table_name):
            print('already')
        else:
            table_service.create_table(table_name)
            print('ok')