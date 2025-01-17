using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public Canvas mainMenuCanvas;
    [SerializeField] public GameObject player;
    private ScriptableRendererData _scriptableRendererData;
    [SerializeField] private Slider pixelSlider;
    
    
    private void  ExtractScriptableRendererData()
    {
        var pipeline = ((UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset);
        FieldInfo propertyInfo = pipeline.GetType(  ).GetField( "m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic );
        _scriptableRendererData = ((ScriptableRendererData[]) propertyInfo?.GetValue( pipeline ))?[0];
    }
    
    void Start()
    {
        mainMenuCanvas.enabled = false;
        player.GetComponent<SpiderMovementController>().enabled = true;
        
        ExtractScriptableRendererData();
        foreach (var renderObjSetting in _scriptableRendererData.rendererFeatures.OfType<PixelizeFeature>())
        {
            renderObjSetting.settings.screenHeight = 600;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenuCanvas.enabled)
            {
                ResumeGame();
            }
            else
            {
                player.GetComponent<SpiderMovementController>().enabled = false;
                mainMenuCanvas.enabled = true;
            }
        }
    }
    
    public void setScreenPixelValue()
    {
        foreach ( var renderObjSetting in _scriptableRendererData.rendererFeatures.OfType<PixelizeFeature>())
        {
            renderObjSetting.settings.screenHeight = (int)pixelSlider.value;
        }
    }
    
    public void ResumeGame()
    {
        if (mainMenuCanvas.enabled)
        {
            player.GetComponent<SpiderMovementController>().enabled = true;
            mainMenuCanvas.enabled = false;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
