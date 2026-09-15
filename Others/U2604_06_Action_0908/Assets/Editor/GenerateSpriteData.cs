using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateSpriteData
{
    [MenuItem("Jobs/Generate Sprite Data %#F9", priority = 20)]
    public static void Generate()
    {
        Texture2D[] textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        
        if (textures.Length < 1)
        {
            Debug.LogWarning("텍스처가 선택되지 않음");
            return;
        }


        //텍스처 설정 변경
        foreach(Texture2D texture in textures)
        {
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if(importer == null)
            {                 
                Debug.LogWarning($"TextureImporter를 가져올 수 없음: {assetPath}");
                continue;
            }


            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.isReadable = true;
            importer.filterMode = FilterMode.Point;

            //Texture Importer Settings
            {
                TextureImporterSettings settings = new TextureImporterSettings();
                importer.ReadTextureSettings(settings);
                settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
                importer.SetTextureSettings(settings);
            }
            
            importer.SaveAndReimport();
        }


        //스프라이트 리스트 만들기
        string path = "";

        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D texture in textures)
        {
            path = AssetDatabase.GetAssetPath(texture);
            
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            sprites.Add(sprite);
        }
        sprites.Sort((a, b) => a.name.CompareTo(b.name));


        //충돌 정보 생성
        ColliderData colliderData = ScriptableObject.CreateInstance<ColliderData>();

        Debug.Log($"{sprites.Count}개의 스프라이트에 대한 처리 시작");
        foreach (Sprite sprite in sprites)
        {
            Rect colliderRect = GetColliderRect(sprite);

            Vector2 size, offset;
            GetColliderSize(sprite, colliderRect, out size, out offset);


            SpriteHitboxData hitboxData = new SpriteHitboxData();
            hitboxData.sprite = sprite;
            hitboxData.offset = offset;
            hitboxData.size = size;

            colliderData.Sprites.Add(hitboxData);
        }

        
        string[] names = sprites[0].name.Split('_');

        //SO 파일 생성
        {
            string soSavePath = "Assets/Resources/Animations/";

            string soName = $"{names[0]}_{names[1]}.asset";
            soSavePath += soName;

            AssetDatabase.CreateAsset(colliderData, soSavePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(soSavePath + " 파일 생성 완료!");
        }
        
        
        //애니메이션 클립 생성
        AnimationClip clip = new AnimationClip();
        clip.frameRate = 12.0f;

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
        for(int i = 0; i < sprites.Count; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe();
            keyframes[i].time = i / clip.frameRate;
            keyframes[i].value = sprites[i];

            //Debug.Log(sprites[i].name);
        }

        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);


        //anim 파일 생성
        {
            string animSavePath = "Assets/Animations/";

            string animName = $"{names[0]}_{names[1]}.anim";
            animSavePath += animName;

            AssetDatabase.CreateAsset(clip, animSavePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(animSavePath + " 파일 생성 완료!");
        }

        //Read/Write 해제
        foreach (Texture2D texture in textures)
        {
            string assetPath = AssetDatabase.GetAssetPath(texture);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            importer.isReadable = false;

            importer.SaveAndReimport();
        }
    }

    private static Rect GetColliderRect(Sprite sprite)
    {
        Texture2D texture = sprite.texture;
        Color[] pixels = texture.GetPixels();


        int width = (int)sprite.rect.width;
        int height = (int)sprite.rect.height;

        int minX = width;
        int maxX = 0;
        int minY = height;
        int maxY = 0;

        List<Color> colors = new List<Color>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (pixels[y * width + x].a > 0.1f)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;

                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;

                    colors.Add(pixels[y * width + x]);
                }
            }//for(x)
        }//for(y)

        if (minX >= maxX || minY >= maxY)
        {
            Debug.LogWarning("잘못된 스프라이트!!");

            return Rect.zero;
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    private static void GetColliderSize(Sprite sprite, Rect rect, out Vector2 size, out Vector2 offset)
    {
        float ppu = sprite.pixelsPerUnit;

        size = new Vector2(rect.width / ppu, rect.height / ppu);

        float x = (rect.center.x - sprite.pivot.x) / ppu;
        float y = (rect.center.y - sprite.pivot.y) / ppu;

        offset = new Vector2(x, y);
    }
}
