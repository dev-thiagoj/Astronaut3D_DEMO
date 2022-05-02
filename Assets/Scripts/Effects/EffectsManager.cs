using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Ebac.Core.Singleton;

public class EffectsManager : Singleton<EffectsManager>
{
    public PostProcessVolume processVolume;
    public float duration = 1f;
    [SerializeField] private Vignette _vignette;


    [NaughtyAttributes.Button]
    public void ChangeVignette()
    {
        StartCoroutine(FlashColorVignette());

    }

    IEnumerator FlashColorVignette()
    {
        Vignette temp;

        if (processVolume.profile.TryGetSettings<Vignette>(out temp)) //usa-se o out para não criar uma variável nova, mas usar no método a que já tem
        {
            _vignette = temp;
        }

        //_vignette.color = Color.red; //não podemos usar direto o Color.Red pois o post processing trabalha com parametro de cor, precisamos definir antes o parametro, então:

        ColorParameter c = new ColorParameter();

        float time = 0;
        while (time < duration)
        {
            c.value = Color.Lerp(Color.white, Color.red, time / duration);
            time += Time.deltaTime;
            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }
        
        time = 0;
        while (time < duration)
        {
            c.value = Color.Lerp(Color.red, Color.white, time / duration);
            time += Time.deltaTime;
            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }
    }
}
