//Using
global using BepInEx;
global using BepInEx.IL2CPP;
global using UnityEngine;
global using UnityEngine.UI;
global using UnhollowerRuntimeLib;
global using HarmonyLib;
global using System;
global using System.Collections.Generic;
global using System.IO;
global using TMPro;
global using System.Globalization;

namespace Exemple
{
    [BepInPlugin("AutoReady", "AutoReady", "1.0.0")]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            ClassInjector.RegisterTypeInIl2Cpp<AutoReady>();
            Harmony.CreateAndPatchAll(typeof(Plugin));

            Utility.CreateFolder(Variables.mainFolderPath, Variables.logFilePath);

            Utility.CreateFile(Variables.logFilePath, Variables.logFilePath);
            Utility.SetConfigFile(Variables.configFilePath);

            Utility.Log(Variables.logFilePath, "Mod Loaded!");
            Utility.Log(Variables.logFilePath, "Mod Created by Gibson!");
        }

        public class AutoReady : MonoBehaviour
        {
            private bool isReady;
            private float time;

            void Update()
            {
                time += Time.deltaTime;
                if (LobbyManager.Instance.gameMode.id == 0 && !isReady && time > 3)
                {
                    Utility.ReadConfigFile();

                    if (Variables.autoReady != true)
                    {
                        isReady = true;
                        return;
                    }

                    PressReadyButton();
                    isReady = true;
                }
            }
            void PressReadyButton()
            {
                GameObject.Find("Button/Button").GetComponent<MonoBehaviour1PublicTrbuObreunObBoVeVeVeUnique>().field_Private_Boolean_0 = true;
                GameObject.Find("Button/Button").GetComponent<MonoBehaviour1PublicTrbuObreunObBoVeVeVeUnique>().TryInteract();

                ChatBox.Instance.ForceMessage("<color=yellow>AutoReady done!</color>");
            }
        }

        //NoMoreScreenShake by Lualt
        [HarmonyPatch(typeof(Shake), nameof(Shake.Awake))]
        [HarmonyPostfix]
        public static void RemoveMoreShake()
        {
            GameObject.Destroy(GameObject.Find("Camera/Recoil/Shake").GetComponent<MilkShake.Shaker>());
        }

        [HarmonyPatch(typeof(CamBob), nameof(CamBob.BobOnce))]
        [HarmonyPatch(typeof(Recoil), nameof(Recoil.AddRecoil))]
        [HarmonyPrefix]
        public static bool NoMoreShaking()
        {
            return false;
        }
        //NoMoreScreenShake by Lualt

        //WhatServerIAmIn by Lualt
        public static GameObject ServerName;
        public static TextMeshProUGUI ServerNameText;
        public static GameObject PlayerCount;
        [HarmonyPatch(typeof(GameUI), "Start")]
        [HarmonyPostfix]
        public static void Start(GameUI __instance)
        {
            if (!PlayerCount)
            {
                PlayerCount = GameObject.Find("GameUI/PlayerList/WindowUI/Header/Tabs/PLayerInLobby");
            }

            if (!GameObject.Find("GameUI/PlayerList/WindowUI/Header/Tabs/ServerName"))
            {
                ServerName = UnityEngine.Object.Instantiate(PlayerCount);
                ServerName.name = "ServerName";

                UnityEngine.Object.Destroy(ServerName.GetComponent<MonoBehaviourPublicTeplUnique>());

                RectTransform ServerNameRect = ServerName.GetComponent<RectTransform>();
                ServerNameRect.sizeDelta = new Vector2(600, 80);

                ServerNameText = ServerName.GetComponent<TextMeshProUGUI>();
                ServerNameText.text = "Couldn't load name.";
                ServerName.transform.parent = PlayerCount.transform.parent;
            }
        }
        [HarmonyPatch(typeof(PlayerList), nameof(PlayerList.UpdateList))]
        [HarmonyFinalizer]
        public static void UpdateList(PlayerList __instance)
        {
            if (ServerNameText)
            {
                ServerNameText.SetText(SteamworksNative.SteamMatchmaking.GetLobbyData(SteamManager.Instance.currentLobby, "LobbyName"));
            }
            else
            {
                Start(GameUI.Instance);
            }
        }
        //WhatServerIAmIn by Lualt

        [HarmonyPatch(typeof(GameUI), "Awake")]
        [HarmonyPostfix]
        public static void UIAwakePatch(GameUI __instance)
        {
            GameObject menuObject = new GameObject();
            Text text = menuObject.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 16;
            text.raycastTarget = false;

            AutoReady exemple = menuObject.AddComponent<AutoReady>();

            menuObject.transform.SetParent(__instance.transform);
            menuObject.transform.localPosition = new UnityEngine.Vector3(menuObject.transform.localPosition.x, -menuObject.transform.localPosition.y, menuObject.transform.localPosition.z);
            RectTransform rt = menuObject.GetComponent<RectTransform>();
            rt.pivot = new UnityEngine.Vector2(0, 1);
            rt.sizeDelta = new UnityEngine.Vector2(1920, 1080);
        }

        //Anticheat Bypass 
        [HarmonyPatch(typeof(EffectManager), "Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0")]
        [HarmonyPatch(typeof(LobbyManager), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(MonoBehaviourPublicVesnUnique), "Method_Private_Void_0")]
        [HarmonyPatch(typeof(LobbySettings), "Method_Public_Void_PDM_2")]
        [HarmonyPatch(typeof(MonoBehaviourPublicTeplUnique), "Method_Private_Void_PDM_32")]
        [HarmonyPrefix]
        public static bool Prefix(System.Reflection.MethodBase __originalMethod)
        {
            return false;
        }
    }
}