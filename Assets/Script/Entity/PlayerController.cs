using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : BaseController
{
    private Camera camera;
    private GameManager gameManager;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        camera = Camera.main;
    }

    protected override void HandleAction()
    {

    }

    public override void Death()
    {
        base.Death();
        gameManager.GameOver();
    }

    void OnMove(InputValue inputValue)
    {
        movementDirection = inputValue.Get<Vector2>();
        // float horizontal = Input.GetAxisRaw("Horizontal"); // 키 입력을 받아오는 것. Horizontal은 A (왼쪽), D (오른쪽), ←, →
        // float vertical = Input.GetAxisRaw("Vertical"); // Vertical은 W (위), S (아래), ↑, ↓
        // movementDirection = new Vector2(horizontal, vertical).normalized; // normalized란 백터의 방향은 유지 크기는 1로 만드는 것
        movementDirection = movementDirection.normalized;
    }

    void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(mousePosition); // 이를 안할시 1픽셀당 x는 1씩 전환되므로 화면 오른쪽에 위치해 있을 때 x는 1920이 됨(1920*1080일 경우)
        lookDirection = (worldPos - (Vector2)transform.position); // 플레이어 위치 - 마우스 위치를 바라보는 방향에 저장장

        if (lookDirection.magnitude < .9f) // magnitude는 백터의 크기를 구하는 코드
        {
            lookDirection = Vector2.zero;
        }
        else
        {
            lookDirection = lookDirection.normalized;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if(EventSystem.current.IsPointerOverGameObject()) // UI에 마우스를 올렸을 떈 화살 쏘지 않게 바꿈
            return;

        IsAttacking = inputValue.isPressed;
    }
}

