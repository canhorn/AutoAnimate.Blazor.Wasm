namespace AutoAnimate.Blazor.Wasm.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class Boxes
{
    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    [SupplyParameterFromQuery(Name = "Duration")]
    public int? Duration { get; set; }

    public ElementReference _boxes;
    private IJSObjectReference? _module;
    private IEnumerable<int> _numbers = Enumerable
        .Range(0, 1000)
        .OrderBy(x => Guid.NewGuid().GetHashCode())
        .Distinct()
        .Take(100);

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "https://cdn.jsdelivr.net/npm/@formkit/auto-animate@1.0.0-beta.1/index.min.js"
            );
            await _module.InvokeVoidAsync("default", _boxes, new { duration = Duration ?? 500 });
        }
    }

    private void Randomize()
    {
        _numbers = _numbers.OrderBy(a => Guid.NewGuid().ToString()).ToList();
    }

    private void SetDuration(int duration)
    {
        var path = NavigationManager
            .ToBaseRelativePath(NavigationManager.Uri)
            .Split("?")
            .FirstOrDefault();
        NavigationManager.NavigateTo($"{path}?Duration={duration}", true);
    }
}
