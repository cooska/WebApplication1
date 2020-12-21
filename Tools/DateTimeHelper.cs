using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardapi.Tools {
    public class DateTimeHelper {
		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static long CurrentUnixTimeMillis() {
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}

	}
}
