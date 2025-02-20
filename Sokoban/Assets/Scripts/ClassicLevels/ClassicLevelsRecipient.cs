using Cysharp.Threading.Tasks;
using Data;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ClassicLevels
{
    public class ClassicLevelsRecipient
    {
        [ItemCanBeNull]
        public async UniTask<ClassicLevelModel> GetModel(string levelName)
        {
            var level = await LoadJson(levelName);
            if (level == null) return null;
            var levelModel = JsonConvert.DeserializeObject<ClassicLevelModel>(level);
            return levelModel;
        }


        async UniTask<string> LoadJson(string key)
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(key);
            await locationHandle.Task;

            if (locationHandle.Status != AsyncOperationStatus.Succeeded || locationHandle.Result.Count == 0)
            {
                Addressables.Release(locationHandle);
                return null; 
            }

            var assetHandle = Addressables.LoadAssetAsync<TextAsset>(key);
            await assetHandle.Task;

            if (assetHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Addressables.Release(assetHandle);
                return null; 
            }

            var json = assetHandle.Result.text;
            Addressables.Release(assetHandle); 

            return json;
        }
    }
}