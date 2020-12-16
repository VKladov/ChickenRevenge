using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Obstacle
{
    [SerializeField] private List<StonePiece> _pieces;

    private void OnEnable()
    {
        foreach (StonePiece peace in _pieces)
            peace.Destroyed += OnPieceDestroyed;
    }

    private void OnPieceDestroyed(StonePiece piece)
    {
        _pieces.Remove(piece);

        if (_pieces.Count == 0)
            Destroy();
        else
            _damageParticle.Play();
            
    }
}
