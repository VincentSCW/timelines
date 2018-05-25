from azure.storage.blob import BlockBlobService

class AzureBlobService(object):
    container_name = 'pictureblobs'

    def __init__(self):
        self.__block_blob_service = BlockBlobService(account_name='timelines', 
            account_key='1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==',
            connection_string='DefaultEndpointsProtocol=https;AccountName=timelines;AccountKey=1Xrf8KQwttk+RzGHNBGpMrO6d7llWY6gHpyeSQ+XV6h2N3VEEtTRuSSmvt5+xA3kyQBWND1OAdt8uvMExZz1CA==;EndpointSuffix=core.chinacloudapi.cn')

    def create_container_if_not_exists(self, cname):
        if self.__block_blob_service.exists(cname):
            print('blob container already exists')
        else:
            self.__block_blob_service.create_container(cname)
            print('container created')

    def upload_picture(self):
        self.create_container_if_not_exists(AzureBlobService.container_name)
        #self.__block_blob_service.create_blob_from_stream(AzureBlobService.container_name, 'test')
        
        