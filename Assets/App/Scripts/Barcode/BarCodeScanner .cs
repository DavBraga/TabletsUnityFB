using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

// creditos a FireDragonGameStudio
// credits to FireDragonGameStudio
// https://github.com/FireDragonGameStudio/Unity-ZXing-BarQrCodeHandling.git
public class BarCodeScanner : MonoBehaviour {



    private WebCamTexture camTexture;
    private Thread qrThread;

    private Color32[] colorpixel;
    private int width, height;

    private Rect screenRect;

    private bool isQuit;

    public string LastResult;

    public string resultData{private set; get;}
    private bool shouldEncodeNow;
    private bool shouldDecodeNow;

    [Header("SlowDown")]
    private bool DoneCooling = true;
    [SerializeField][Tooltip("More is less peformant and less demanding. Reduce if its failing too much .")]
    int FramesToWait = 5;

    Coroutine frameSkipRoutine;

    [Header("Frames per second")]
    [SerializeField] Text fpsText;
    float deltaTime;

    [Header("Visuals")]
    [SerializeField] RawImage cameraShow;
    [SerializeField] Text extractedData;

    [Header("QR code Generation")]
    [SerializeField]
    bool generatingQRcodes = false;
    // Texture for encoding test
    public Texture2D encoded;

    void OnGUI() {
    }

    void OnEnable() {
        if (camTexture != null) {
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
        DoneCooling = true;

    }

    void OnDisable() {
        if (camTexture != null) {
            camTexture.Pause();
        }
        StopCoroutine(frameSkipRoutine);

        resultData = "";
        extractedData.text = resultData;
    }

    void OnDestroy() {
        qrThread.Abort();
        camTexture.Stop();
    }

    // It's better to stop the thread by itself rather than abort it.
    void OnApplicationQuit() {
        isQuit = true;
    }

    void Start() {
        encoded = new Texture2D(256, 256);
        LastResult = "http://www.google.com";
        shouldEncodeNow = true;

        screenRect = new Rect(0, 0, Screen.width, Screen.height);

        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height; // 480;
        camTexture.requestedWidth = Screen.width; //640;
        cameraShow.texture = camTexture;
        OnEnable();

        colorpixel = new Color32[width * height];

        qrThread = new Thread(DecodeQR);
        qrThread.Start();
        AdjustRotation();
    }

    void Update()
    {

        if (!DoneCooling) return;
        DoneCooling = false;
        FramesPerSecond();
        frameSkipRoutine = StartCoroutine(WaitFrames(FramesToWait));

        cameraShow.rectTransform.sizeDelta = new Vector2(camTexture.width, camTexture.height);
        if (!shouldDecodeNow)
        {
            camTexture.GetPixels32(colorpixel);
            shouldDecodeNow = !shouldDecodeNow;
        }

        // Não estou gerando novos QR codes;

        // encode the last found
        if(generatingQRcodes) GenerateQRCode();
    }

    private void GenerateQRCode()
    {
        var textForEncoding = LastResult;
        if (shouldEncodeNow &&
            textForEncoding != null)
        {
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            shouldEncodeNow = false;
        }
    }

    void DecodeQR() {
        // create a reader with a custom luminance source
        var barcodeReader = new BarcodeReader {
            AutoRotate = false,
            Options = new ZXing.Common.DecodingOptions {
                TryHarder = true
            }
        };

        while (true) {
            if (isQuit)
                break;

            try {
                // decode the current frame
                var result = barcodeReader.Decode(colorpixel, width, height);
                if (result != null) {
                    resultData = result.Text;
                    extractedData.text = resultData;
                    shouldEncodeNow = true;
                    print(result.Text + " " + result.BarcodeFormat);
                }

                // Sleep a little bit and set the signal to get the next frame
                Thread.Sleep(200);
                shouldDecodeNow = false;
            } catch {
            }
        }
    }

    // transforma o valor em um qrcode.
    private static Color32[] Encode(string textForEncoding, int width, int height) {
        var writer = new BarcodeWriter {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

     private void AdjustRotation()
    {
        int rotationAngle = 0;

        switch (Screen.orientation)
        {
            case ScreenOrientation.Portrait:
                rotationAngle = -90;
                break;
            case ScreenOrientation.LandscapeLeft:
                rotationAngle = 0;
                break;
            case ScreenOrientation.LandscapeRight:
                rotationAngle = 180;
                break;
            case ScreenOrientation.PortraitUpsideDown:
                rotationAngle = 90;
                break;
        }

        cameraShow.transform.eulerAngles = new Vector3(0, 0, rotationAngle);
    }

    public void FramesPerSecond()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		fpsText.text = Mathf.Ceil (fps).ToString ();
    }
    private IEnumerator WaitFrames(int FramesToWait)
    {
        int frameCounter = 0;
        while (frameCounter<FramesToWait)
        {
            // Wait until the end of the frame
            yield return new WaitForEndOfFrame();

            // Increment the frame counter
            frameCounter++;
             Debug.Log("Current frame count:"+ frameCounter);
            // Check if the desired number of frames has passed
        }

        DoneCooling = true;
        Debug.Log("frame count done:"+ frameCounter);
        // Reset the frame counter
         frameCounter = 0;
        
    }

}