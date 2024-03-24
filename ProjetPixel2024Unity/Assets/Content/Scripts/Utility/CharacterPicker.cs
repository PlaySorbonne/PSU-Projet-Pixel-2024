using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Component.Spawning;
using FishNet.Object;

public class CharacterPicker : MonoBehaviour
{
    public enum CharactersEnum : int
    {
        Crab, Squid, Fish, Shrimp, Slug, LUCA
    }

    public CharactersEnum currentCharacter;


    public NetworkObject crabPrefab;
    public NetworkObject squidPrefab;
    public NetworkObject fishPrefab;
    public NetworkObject shrimpPrefab;
    public NetworkObject slugPrefab;
    public NetworkObject lucaPrefab;

    private PlayerSpawner _playerSpawner;

    public NetworkObject GetCharacterPrefab(CharactersEnum character)
    {
        switch (character)
        {
            case CharactersEnum.Crab:
                return crabPrefab;
            case CharactersEnum.Squid:
                return squidPrefab;
            case CharactersEnum.Fish:
                return fishPrefab;
            case CharactersEnum.Shrimp:
                return shrimpPrefab;
            case CharactersEnum.Slug:
                return slugPrefab;
            case CharactersEnum.LUCA:
                return lucaPrefab;
            default:
                return crabPrefab;
        }
    }

    void Start()
    {
        _playerSpawner = FindObjectOfType<PlayerSpawner>();
        _playerSpawner._playerPrefab = GetCharacterPrefab(currentCharacter);
    }
}
