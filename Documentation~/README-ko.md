[Portuguese](../README.md) | [English](./README-en.md)

# ASPax 핸들러

## 개요

**ASPax 핸들러**는 Unity 엔진용 C# 스크립트 모음으로, `Animator` 및 `Transform`과 같은 특정 컴포넌트의 조작을 단순화하고 향상시키기 위해 설계되었습니다. 주요 목표는 복잡한 기능에 대한 안전하고 효율적이며 사용하기 쉬운 액세스를 제공하고 런타임 성능을 최적화하는 것입니다.

이 패키지는 워크플로를 간소화하고 코드의 장황함을 줄이며 Unity 컴포넌트와의 상호 작용이 더 강력하고 오류 발생 가능성이 적도록 보장하려는 개발자에게 이상적입니다.

## 기능

### AnimatorHandler

`AnimatorHandler` 클래스는 `Animator` 컴포넌트의 속성에 액세스하고 관리하는 구조화된 방법을 제공합니다.

> **중요:** 이 클래스는 인스턴스화(`new`)해야 하며 GameObject에 컴포넌트로 추가할 수 없습니다. 따라서 `GetComponent`를 사용하여 액세스할 수 없습니다.

**인스턴스화 예제:**

`Start()` 또는 `Awake()` 메서드에서 클래스를 인스턴스화하는 것이 좋습니다.

```csharp
public Animator animator;
public ASPax.Handlers.AnimatorHandler animatorHandler;

void Awake()
{
    animator = GetComponent<Animator>();
}

void Start()
{
    animatorHandler = new(animator);
}
```

**주요 기능:**

- **`Animator`**: 연결된 `Animator` 컴포넌트를 반환합니다.
- **`Parameters`**: `AnimatorControllerParameter` 배열을 반환하여 모든 컨트롤러 매개변수에 액세스할 수 있습니다.
- **`ParameterHandlers`**: `ParameterHandler` (구조체) 배열을 반환하여 이름(문자열) 대신 ID를 사용하여 `Animator` 매개변수를 조작함으로써 성능을 향상시킵니다.
- **`AnimationClips`**: `Animator Controller`에 있는 모든 `AnimationClip`이 포함된 배열을 반환합니다.
- **`IsNecessaryUpdateInstance()`**: 매개변수나 애니메이션 클립이 변경된 경우와 같이 `AnimatorHandler` 인스턴스를 업데이트해야 하는지 확인합니다.

#### `ParameterHandler` 구조체

이 구조체는 `Animator` 매개변수에 대한 액세스를 최적화하기 위해 만들어졌습니다. 런타임에 비용이 많이 드는 문자열 비교를 피하기 위해 매개변수 이름(`string`) 대신 ID(`int`)를 사용합니다.

**기능:**

- **`Name`**: 매개변수의 이름입니다.
- **`ID`**: 매개변수 이름을 효율적으로 식별하는 데 사용되는 해시입니다.

**사용 예제:**

```csharp
public Animator animator;
public ASPax.Handlers.AnimatorHandler.ParameterHandler animatorParameter = new("isOpen");

void Awake()
{
    animator = GetComponent<Animator>();
}

public void ToggleAnimation(bool isOpen)
{
    animator.SetBool(animatorParameter.ID, isOpen);
}
```

### TransformHandler

`TransformHandler`는 `UIBehaviour`를 상속하는 컴포넌트로, `Transform` 및 `RectTransform` 속성에 대한 액세스 및 조작을 용이하게 하기 위해 GameObject에 추가할 수 있습니다. 편집 모드와 런타임(`ExecuteAlways`) 모두에서 작동합니다.

**주요 기능:**

- **안전한 속성 액세스:** `Position`, `Rotation`, `Scale` 및 기타 `Transform` 속성에 대한 안전한 액세스를 제공합니다. `TransformHandler`에는 예기치 않은 동작을 유발할 수 있는 `NaN`(Not a Number) 값을 방지하기 위한 검사가 포함되어 있습니다.
- **`RectTransform` 지원:** GameObject에 `RectTransform`이 있는지 자동으로 감지하고 `AnchoredPosition` 및 `Ratio` 계산과 같은 특정 기능을 활성화합니다.
- **좌표 변환:** 화면 좌표와 월드 좌표 간에 위치를 변환하는 메서드를 포함합니다.
- **비율 계산:** `RectTransform`의 경우 너비와 높이 사이의 `Ratio`를 계산하며, 다양한 레이아웃 요구에 맞게 조정 가능한 `RangeRatioHandler`가 있습니다.
- **실시간 업데이트:** 변경 사항이 있을 때 속성이 Inspector에서 자동으로 업데이트되어 디버깅 및 시각화가 용이합니다.

## 프로젝트 목표

- **상호 작용 단순화:** Unity 컴포넌트와 상호 작용할 때의 복잡성을 줄입니다.
- **성능 향상:** ID로 애니메이션 매개변수에 액세스하는 것과 같은 더 효율적인 방법의 사용을 우선시합니다.
- **안전성 향상:** 벡터 및 쿼터니언의 `NaN` 값과 같은 일반적인 오류를 방지하기 위한 검사를 구현합니다.
- **디버깅 용이성:** 우발적인 수정을 방지하기 위해 `ReadOnly` 필드를 사용하여 관련성 있고 읽기 쉬운 정보를 Inspector에 표시합니다.

## 참고

- **직렬화:** `AnimatorHandler`와 `TransformHandler`는 모두 직렬화 가능하므로 해당 속성을 Unity Inspector에서 볼 수 있습니다.
- **사용자 지정 특성:** 이 프로젝트는 `ASPax.Attributes` 라이브러리를 사용하여 `ReadOnly`, `ShowIf`, `HorizontalLine`과 같은 기능으로 Inspector를 풍부하게 하여 정보의 구성과 명확성을 향상시킵니다.
- **종속성:** 이 패키지는 `com.adrianuspax.attributes`에 대한 종속성이 있으며, 사용자 지정 특성이 올바르게 작동하려면 설치해야 합니다.
- **편집 모드:** `TransformHandler`는 Unity의 편집 모드에서 작동하도록 설계되어 개발자가 씬을 실행할 필요 없이 실시간으로 변경 사항을 볼 수 있습니다.

## 시각적 예제

아래 이미지는 `AnimatorHandler`가 Inspector에 애니메이션 매개변수와 클립을 표시하여 각 요소의 인덱스를 쉽게 식별할 수 있도록 하는 방법을 보여줍니다.

![Screenshot](./Images/Screenshot01.png)
