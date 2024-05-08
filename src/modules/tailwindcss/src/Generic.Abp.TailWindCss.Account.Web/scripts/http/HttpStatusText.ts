export interface HttpStatusTextType {
    [key: string]: {
        [key: string]: string
    }
}

export const HttpStatusText = {
    "zh-CN": {
        "retrying": "正在重试，还可以尝试 {retryCount} 次，每隔 {retryInterval} 秒重试一次，如果不想再重试，可以取消请求。",
        "retryFailed": "重试失败",
        "unknownError": "未知错误",
        "400": "请求错误",
        "401": "未授权",
        "403": "禁止访问",
        "404": "未找到",
        "405": "方法不允许",
        "408": "请求超时",
        "413": "请求实体过大",
        "414": "请求URI过长",
        "415": "不支持的媒体类型",
        "416": "请求范围不符合要求",
        "417": "期待失败",
        "500": "服务器内部错误",
        "501": "尚未实施",
        "502": "网关错误",
        "503": "服务不可用",
        "504": "网关超时",
        "505": "HTTP版本不受支持"
    },
    "en-US": {
        "retrying": "Retrying, you have {retryCount} attempts left, retrying every {retryInterval} seconds. If you don't want to retry, you can cancel the request.",
        "retryFailed": "Retry Failed",
        "unknownError": "Unknown Error",
        "400": "Bad Request",
        "401": "Unauthorized",
        "403": "Forbidden",
        "404": "Not Found",
        "405": "Method Not Allowed",
        "408": "Request Timeout",
        "413": "Request Entity Too Large",
        "414": "Request-URI Too Long",
        "415": "Unsupported Media Type",
        "416": "Requested Range Not Satisfiable",
        "417": "Expectation Failed",
        "500": "Internal Server Error",
        "501": "Not Implemented",
        "502": "Bad Gateway",
        "503": "Service Unavailable",
        "504": "Gateway Timeout",
        "505": "HTTP Version Not Supported"
    },
    "zh-TW": {
        "retrying": "正在重試，還有 {retryCount} 次嘗試機會，每隔 {retryInterval} 秒重試一次，如果不想再重試，可以取消請求。",
        "retryFailed": "重試失敗",
        "unknownError": "未知錯誤",
        "400": "請求錯誤",
        "401": "未授權",
        "403": "禁止訪問",
        "404": "找不到",
        "405": "方法不允許",
        "408": "請求超時",
        "413": "請求實體過大",
        "414": "請求URI過長",
        "415": "不支援的媒體類型",
        "416": "請求範圍不符合要求",
        "417": "預期失敗",
        "500": "伺服器內部錯誤",
        "501": "尚未實施",
        "502": "閘道錯誤",
        "503": "服務不可用",
        "504": "閘道超時",
        "505": "HTTP版本不支援"
    }
}

