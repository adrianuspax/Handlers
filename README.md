[English](./Documentation~/README-en.md) | [Korean](./Documentation~/README-ko.md)

# ASPax Handlers

## Visão Geral

O **ASPax Handlers** é uma coleção de scripts em C# para a Unity Engine, projetada para simplificar e aprimorar a manipulação de componentes específicos, como `Animator` e `Transform`. O objetivo principal é fornecer acesso seguro, eficiente e fácil de usar a funcionalidades complexas, além de otimizar a performance em tempo de execução.

Este pacote é ideal para desenvolvedores que buscam agilizar o fluxo de trabalho, reduzir a verbosidade do código e garantir que as interações com os componentes da Unity sejam mais robustas e menos propensas a erros.

## Funcionalidades

### AnimatorHandler

A classe `AnimatorHandler` oferece uma maneira estruturada de acessar e gerenciar as propriedades de um componente `Animator`.

> **Importante:** Esta classe precisa ser instanciada (`new`) e não pode ser adicionada como um componente a um GameObject. Portanto, não é possível usar `GetComponent` para acessá-la.

**Exemplo de Instanciação:**

É recomendável instanciar a classe no método `Start()` ou `Awake()`:

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

**Recursos Principais:**

- **`Animator`**: Retorna o componente `Animator` associado.
- **`Parameters`**: Retorna um array de `AnimatorControllerParameter`, permitindo acesso a todos os parâmetros do controller.
- **`ParameterHandlers`**: Retorna um array de `ParameterHandler` (struct), que facilita a manipulação dos parâmetros do `Animator` usando seus IDs em vez de nomes (strings), o que melhora a performance.
- **`AnimationClips`**: Retorna um array com todos os `AnimationClip` presentes no `Animator Controller`.
- **`IsNecessaryUpdateInstance()`**: Verifica se a instância do `AnimatorHandler` precisa ser atualizada, por exemplo, se os parâmetros ou clipes de animação foram alterados.

#### Struct `ParameterHandler`

Este struct foi criado para otimizar o acesso aos parâmetros do `Animator`. Ao usar o ID do parâmetro (`int`) em vez do nome (`string`), evitam-se comparações de strings custosas em tempo de execução.

**Recursos:**

- **`Name`**: O nome do parâmetro.
- **`ID`**: O hash do nome do parâmetro, usado para identificá-lo de forma eficiente.

**Exemplo de Uso:**

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

O `TransformHandler` é um componente que herda de `UIBehaviour` e pode ser adicionado a um GameObject para facilitar o acesso e a manipulação de propriedades do `Transform` e `RectTransform`. Ele funciona tanto em modo de edição quanto em tempo de execução (`ExecuteAlways`).

**Recursos Principais:**

- **Acesso Seguro a Propriedades:** Fornece acesso seguro a `Position`, `Rotation`, `Scale` e outras propriedades do `Transform`. O `TransformHandler` inclui verificações para evitar valores `NaN` (Not a Number), que podem causar comportamentos inesperados.
- **Suporte a `RectTransform`:** Detecta automaticamente se o GameObject possui um `RectTransform` e habilita funcionalidades específicas, como `AnchoredPosition` e cálculos de `Ratio`.
- **Conversão de Coordenadas:** Inclui métodos para converter posições entre coordenadas de tela (Screen) e de mundo (World).
- **Cálculo de Proporção (Ratio):** Para `RectTransform`, calcula a proporção (`Ratio`) entre a largura e a altura, com um `RangeRatioHandler` ajustável para diferentes necessidades de layout.
- **Atualização em Tempo Real:** As propriedades são atualizadas automaticamente no Inspector quando há mudanças, facilitando o debug e a visualização.

## Objetivos do Projeto

- **Simplificar a Interação:** Reduzir a complexidade ao interagir com componentes da Unity.
- **Melhorar a Performance:** Priorizar o uso de métodos mais eficientes, como o acesso a parâmetros de animação por ID.
- **Aumentar a Segurança:** Implementar verificações para prevenir erros comuns, como valores `NaN` em vetores e quatérnios.
- **Facilitar o Debug:** Exibir informações relevantes e de fácil leitura no Inspector, com campos `ReadOnly` para evitar modificações acidentais.

## Observações

- **Serialização:** Tanto o `AnimatorHandler` quanto o `TransformHandler` são serializáveis, o que significa que suas propriedades podem ser visualizadas no Inspector da Unity.
- **Atributos Customizados:** O projeto utiliza a biblioteca `ASPax.Attributes`, que enriquece o Inspector com funcionalidades como `ReadOnly`, `ShowIf`, e `HorizontalLine`, melhorando a organização e a clareza das informações.
- **Dependências:** Este pacote possui uma dependência do `com.adrianuspax.attributes`, que deve ser instalado para que os atributos customizados funcionem corretamente.
- **Modo de Edição:** O `TransformHandler` foi projetado para funcionar no modo de edição da Unity, permitindo que os desenvolvedores visualizem as mudanças em tempo real sem a necessidade de executar a cena.

## Exemplo Visual

A imagem abaixo demonstra como o `AnimatorHandler` exibe os parâmetros e clipes de animação no Inspector, facilitando a identificação dos índices de cada elemento.

![Screenshot](./Documentation~/Images/Screenshot01.png)
