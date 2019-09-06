<Query Kind="Program">
  <Reference Relative="bin\Release\OperatorJumble.dll"></Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework\v4.0.30319\WPF\PresentationCore.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework\v4.0.30319\WPF\PresentationFramework.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Xaml.dll</Reference>
  <Reference>C:\Windows\Microsoft.NET\Framework\v4.0.30319\WPF\WindowsBase.dll</Reference>
  <Namespace>OperatorJumble</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Windows</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
  <CopyLocal>true</CopyLocal>
</Query>

void Main()
{
    var opjum = new MattTreeSearch();
    var vals = opjum.BuildCompDict(9);
    
    var solns = vals.Where(kv => kv.Key.val > 0)
                    .Where(kv => (kv.Key.max - kv.Key.min) == 8)
                    .OrderBy(kv => kv.Key.val)
                    .Take(11111);
    foreach (var s in solns)
    {
        $"{s.Key.val}, {s.Value}".Dump();
    }
}

// Define other methods and classes here
