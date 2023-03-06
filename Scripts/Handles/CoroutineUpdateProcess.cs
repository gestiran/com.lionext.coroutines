using System.Collections;
using System.Collections.Generic;

namespace Lionext.Coroutines.Handles {
    public delegate bool CoroutineUpdateProcess(ref Stack<IEnumerator> stack, ref IEnumerator enumerator);
}