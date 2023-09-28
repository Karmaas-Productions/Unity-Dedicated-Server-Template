using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class ServerBrowserUI : MonoBehaviour {


    [SerializeField] private Transform serverContainer;
    [SerializeField] private Transform serverTemplate;
    [SerializeField] private Button joinIPButton;
    [SerializeField] private Button createServerButton;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField portInputField;


    private void Awake() {
        joinIPButton.onClick.AddListener(() => {
            string ipv4Address = ipInputField.text;
            ushort port = ushort.Parse(portInputField.text);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipv4Address, port);

            KitchenGameMultiplayer.Instance.StartClient();
        });

        createServerButton.onClick.AddListener(() => {
            string keyId = "AAAAAAAAAAAAAAAA";
            string keySecret = "AAAAAAAAAAAAAAAA";
            byte[] keyByteArray = Encoding.UTF8.GetBytes(keyId + ":" + keySecret);
            string keyBase64 = Convert.ToBase64String(keyByteArray);

            string projectId = "AAAAAAAAAAAAAAAA";
            string environmentId = "AAAAAAAAAAAAAAAA";
            string url = $"https://services.api.unity.com/auth/v1/token-exchange?projectId={projectId}&environmentId={environmentId}";

            string jsonRequestBody = JsonUtility.ToJson(new TokenExchangeRequest {
                scopes = new[] { "multiplay.allocations.create", "multiplay.allocations.list" },
            });

            WebRequests.PostJson(url,
            (UnityWebRequest unityWebRequest) => {
                unityWebRequest.SetRequestHeader("Authorization", "Basic " + keyBase64);
            },
            jsonRequestBody,
            (string error) => {
                Debug.Log("Error: " + error);
            },
            (string json) => {
                Debug.Log("Success: " + json);
                TokenExchangeResponse tokenExchangeResponse = JsonUtility.FromJson<TokenExchangeResponse>(json);



                string fleetId = "AAAAAAAAAAAAAAAA";
                string url = $"https://multiplay.services.api.unity.com/v1/allocations/projects/{projectId}/environments/{environmentId}/fleets/{fleetId}/allocations";

                WebRequests.PostJson(url,
                (UnityWebRequest unityWebRequest) => {
                    unityWebRequest.SetRequestHeader("Authorization", "Bearer " + tokenExchangeResponse.accessToken);
                },
                JsonUtility.ToJson(new QueueAllocationRequest {
                    allocationId = "AAAAAAAAAAAAAAAA",
                    buildConfigurationId = 0,
                    regionId = "AAAAAAAAAAAAAAAA",
            }),
                (string error) => {
                    Debug.Log("Error: " + error);
                },
                (string json) => {
                    Debug.Log("Success: " + json);
                }
                );
            }
            );
        });

        serverTemplate.gameObject.SetActive(false);
        foreach (Transform child in serverContainer) {
            if (child == serverTemplate) continue;
            Destroy(child.gameObject);
        }
    }

#if !DEDICATED_SERVER
    private void Start() {
        string keyId = "AAAAAAAAAAAAAAAA";
        string keySecret = "AAAAAAAAAAAAAAAA";
        byte[] keyByteArray = Encoding.UTF8.GetBytes(keyId + ":" + keySecret);
        string keyBase64 = Convert.ToBase64String(keyByteArray);

        string projectId = "AAAAAAAAAAAAAAAA";
        string environmentId = "AAAAAAAAAAAAAAAA";
        string url = $"https://services.api.unity.com/multiplay/servers/v1/projects/{projectId}/environments/{environmentId}/servers";

        WebRequests.Get(url,
        (UnityWebRequest unityWebRequest) => {
            unityWebRequest.SetRequestHeader("Authorization", "Basic " + keyBase64);
        },
        (string error) => {
            Debug.Log("Error: " + error);
        },
        (string json) => {
            Debug.Log("Success: " + json);
            ListServers listServers = JsonUtility.FromJson<ListServers>("{\"serverList\":" + json + "}");
            foreach (Server server in listServers.serverList) {
                //Debug.Log(server.ip + " : " + server.port + " " + server.deleted + " " + server.status);
                if (server.status == ServerStatus.ONLINE.ToString() || server.status == ServerStatus.ALLOCATED.ToString()) {
                    // Server is Online!
                    Transform serverTransform = Instantiate(serverTemplate, serverContainer);
                    serverTransform.gameObject.SetActive(true);
                    serverTransform.GetComponent<ServerBrowserSingleUI>().SetServer(
                        server.ip,
                        (ushort)server.port
                    );
                }
            }
        }
        );
    }
#endif

    public class TokenExchangeResponse {
        public string accessToken;
    }


    [Serializable]
    public class TokenExchangeRequest {
        public string[] scopes;
    }

    [Serializable]
    public class QueueAllocationRequest {
        public string allocationId;
        public int buildConfigurationId;
        public string payload;
        public string regionId;
        public bool restart;
    }


    private enum ServerStatus {
        AVAILABLE,
        ONLINE,
        ALLOCATED
    }

    [Serializable]
    public class ListServers {
        public Server[] serverList;
    }

    [Serializable]
    public class Server {
        public int buildConfigurationID;
        public string buildConfigurationName;
        public string buildName;
        public bool deleted;
        public string fleetID;
        public string fleetName;
        public string hardwareType;
        public int id;
        public string ip;
        public int locationID;
        public string locationName;
        public int machineID;
        public int port;
        public string status;
    }

}