// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace Covoiturage.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Covoiturage;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\_Imports.razor"
using Covoiturage.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\Pages\Index.razor"
using System.Net.Http.Headers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\Pages\Index.razor"
using Covoiturage.Payloads.Request;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\Pages\Index.razor"
using Covoiturage.Payloads.Response;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/home")]
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 200 "C:\Users\diarr\source\repos\Covoiturage\Covoiturage\Pages\Index.razor"
      
    private List<OffreRequest> _offres = null;
    private RechercheRequest rechercheRequest = new RechercheRequest();
    private OffreRequest offreRequest = new OffreRequest();
    private VoitureRequest voitureRequest = new VoitureRequest();
    private string escaleRequest;
    private string escaleRequestMotifs;
    private string userId;

    protected override async Task OnInitializedAsync()
    {
        userId = await Js.InvokeAsync<string>("getItemLs", new object[] { "id" });
        var token = await Js.InvokeAsync<string>("getItemLs", new object[] { "token" });
        if (!string.IsNullOrEmpty(token))
        {
            //HttpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            //HttpClient.DefaultRequestHeaders.Add("crossorigin", "true");
            var response = await HttpClient.GetAsync("http://localhost:8080/api/offre/offres");
            _offres = await response.Content.ReadFromJsonAsync<List<OffreRequest>>();
        }
    }

    private async void HandleInteressted(long id)
    {
        var userId = await Js.InvokeAsync<string>("getItemLs", new object[] { "id" });
        if (string.IsNullOrEmpty(userId)) return;
        var response = await HttpClient.PostAsJsonAsync("http://localhost:8080/api/demande/postuler/" + userId + "/" + id, new { userId = userId });
        if (response.IsSuccessStatusCode)
        {
            await Js.InvokeVoidAsync("showToast", "succes", "Opération effectuée avec succès");
        }
        else
        {
            var message = await response.Content.ReadFromJsonAsync<MessageResponse>();
            if (message != null) await Js.InvokeVoidAsync("showToast", "error", message.message);
            else await Js.InvokeVoidAsync("showToast", "error", "Opération échouée");
        }
    }

    private async void HandleRecherche()
    {
        var response = await HttpClient.PostAsJsonAsync<RechercheRequest>("http://localhost:8080/api/offre/recherche", rechercheRequest);
        if (response.IsSuccessStatusCode)
        {
            var temp = await response.Content.ReadFromJsonAsync<List<OffreRequest>>();
            if (temp == null || temp.Count == 0)
                await Js.InvokeVoidAsync("showToast", "error", "Aucun résultat trouvé");
            else
            {
                _offres = temp;
                StateHasChanged();
            }
        }
        else
        {
            await Js.InvokeVoidAsync("showToast", "error", "Aucun résultat trouvé");
        }
    }

    private async void HandleAddOffre()
    {
        var userId = await Js.InvokeAsync<string>("getItemLs", "id");
        offreRequest.escales = new List<EscaleRequest>();
        var escaleLieus = escaleRequest?.Split(";");
        var escaleMotifs = escaleRequestMotifs?.Split(";");

        if (offreRequest.dateDepart <= DateTime.Today)
        {
            await Js.InvokeVoidAsync("showToast", "error", "Choisissez une date valide");
            return;
        }

        if (escaleMotifs != null && escaleLieus != null && escaleMotifs.Length != escaleLieus.Length)
        {
            await Js.InvokeVoidAsync("showToast", "error", "Choisissez un motif pour chaque escale");
            return;
        }
        for(int i = 0; escaleLieus != null && i < escaleLieus.Length; i++)
        {
            offreRequest.escales.Add(new EscaleRequest { lieu = escaleLieus[i], motif = escaleMotifs[i] });
        }
        offreRequest.voiture = voitureRequest;
        offreRequest.offreur = null ;

        Console.WriteLine(userId);

        var response = await HttpClient.PostAsJsonAsync<OffreRequest>("http://localhost:8080/api/offre/ajout/"+userId, offreRequest);
        if (response.IsSuccessStatusCode)
        {
            //var o = await response.Content.ReadFromJsonAsync<OffreRequest>();
            await Js.InvokeVoidAsync("showToast", "succes", "Ajout effectuée avec succès. Rendez vous dans la section 'Mes offres' " +
                "pour voir l'ensemble de vos offres");
            //_offres.Add(o);
            StateHasChanged();
        }
        else
        {
            await Js.InvokeVoidAsync("showToast", "error", "Une erreur est survenue");
        }
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient HttpClient { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime Js { get; set; }
    }
}
#pragma warning restore 1591
