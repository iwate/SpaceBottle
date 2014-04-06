using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

namespace SateliteOrbitCacheWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        // キャッシュ ワーカー ロールをサポートするために追加で必要な実装はありません。 
        // 追加機能は、キャッシュ サービスのパフォーマンスに影響を及ぼす可能性があります。 
        // 専用キャッシュ ワーカー ロールとキャッシュ サービスの詳細については、 
        // MSDN ドキュメント (http://go.microsoft.com/fwlink/?LinkID=247285) を参照してください
    }
}
