using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkConnectionifo
{

    public bool host;//실행 여부

    public string IPAdress;//클라이언트로 실행시 접속할IP주소

    public int port;//클라이언트로 실행시 접속할 포트

}
