# **SOLID**, o que significa?

### Escrito por: Victor Souza  A.k.a _vhost_

**SOLID** é um acrônimo de 5 princípios de boa práticas de arquitetura de _Software_ onde:

**S** corresponde a **SRP** _Single-Responsability-Principle (Princípio da responsabilidade única)_  
**O** corresponde a **OCP** _Open-Closed-Principle (Princípio do aberto/fechado)_  
**L** corresponde a **LSP** _Liskov substitutive principle (Princípio de substituição de Liskov)_  
**I** corresponde a **ISP** _Interface segregation principle (Princípio de segregação de interface)_  
**D** corresponde a **DIP** _Dependency inversion principle (Princípio de inversão de dependência)_

## SRP, o princípio da responsabilidade única. 

O SRP diz que cada classe tem que ter uma única responsabilidade e apenas uma, se a classe é responsável por acessar os dados, então é tudo que ela deve fazer ... acessar dados, não deve tomar decisões de regras de negócio, não deve enviar nada por email ou escrever em arquivos, apenas acessar e devolver dados. É bem comum vermos classes monolíticas enormes que fazem de tudo um pouco, seja em softwares legado ou em softwares que foram mal escritos e feitos com pressa sacrificando qualidade em troca de velocidade, ou em softwares reféns de _Frameworks_ que requerem que um padrão seja seguido, onde esse padrão infringe esses princípios. Existem pessoas que acham isso bom, uma classe que faz tudo, mas pare para observar, toda vez que tiver que ocorrer uma alteração em um ponto específico de uma classe, o risco dessa alteração afetar todos os demais pontos e causar alteração, tendo que replicar código ou até mesmo acabar impedindo o bom funcionamento de outras classes. Com certeza uma classe com muitas responsabilidades não é algo saudável para o programa. E como você pode impedir isso? Simples, se sua classe tem muitas responsabilidades, divida essa responsabilidade em outras classes, com métodos mais específicos, a carga é diminuída e toda vez que tiver que ocorrer alteração, será nas classes específicas, que tem responsabilidades únicas ( o que aumenta a reusabildade, já que podem ser aproveitadas em outros pontos ) porém, há casos em que se tem um método muito grande, mas não o suficiente para que tenha que ir para outra classe, você pode quebrar esse método em métodos privados menores e mais específicos. Observe os exemplos abaixo:

```csharp
 public class RetornoDoJedi
    {
        public string GetCorDoSabrePorNomeDoJedi(string nome)
        {
            //Vai no banco de dados, e faz select na coluna sabre_cor filtrando pelo nome
            //Retorna a cor 
            return "Cor do sabre";
        }

        public void SendCorDoSabrePorEmail(string corDoSabre)
        {
            //Envia cor do sabe por email
        }
    }
```
Repare que nossa classe tem mais de uma responsabilidade, ela deveria apenas acessar os dados do Jedi e nos retornar a cor do sabre, mas ela também envia por email, isso não está correto, para resolvermos isso devemos isolar o metodo que envia por email em outra classe, onde a unica responsabilidade dela vai ser receber um dado e enviar por email.

```csharp
 public class EnviaEmail
    {
        public void SendCorDoSabrePorEmail(string cor)
        {
            //Envia a cor do sabre por email.
        }
    }
```
```csharp
 class Program
    {
        static void Main(string[] args)
        {
            RetornoDoJedi pegaCorDoSabre = new RetornoDoJedi();

            EnviaEmail enviaCorDoSabre = new EnviaEmail();

            string corDoSabre = pegaCorDoSabre.GetCorDoSabrePorNomeDoJedi("Luke");

            enviaCorDoSabre.SendCorDoSabrePorEmail(corDoSabre);
        }
    }
```

Agora, temos 2 classes com responsabilidades únicas, onde qualquer alteração feita em uma não afeta diretamente a outra, pois sua função principal continuará intacta, se a classe **EnviaEmail** parar, a classe **RetornoDoJedi** continua intacta, e vice e versa. 

## OCP, o princípio do _aberto/fechado_.

OCP diz que uma classe tem que estar sempre aberta para estender seu comportamento, mas tem que estar fechada para modificações. Qualquer um que queira se integrar ao código existente tem que usar um interface comum. Imagine que você é dono de uma loja de música, e você tem um programa para quem quiser comprar o áudio, esse programa tem uma classe que recebe o nome de uma música e um meio de pagamento e gera a ordem de pagamento. Observe o código abaixo:

```csharp
    public class GeradorDeOrdensDePagamento
    {
        public void GerarOrdemDePagamento(int IdMusica, long NumeroCartaoDeCredito)
        {
            //geraOrdemDePagamento.
        };
    }
```

Parece correto, não é? Errado, o nosso método recebe um número de cartão de crédito por parâmetro, mas nós temos várias formas de pagamento: _Dinheiro, Voucher, Cheque, Cartão de débito, entre outros..._ Toda vez que quiséssemos adicionar um novo método de pagamento teriámos que modificar o código, e nós temos que nos manter fechados para modificação. **Vamos resolver!**

```csharp
//Criamos uma interface onde todo meio de pagamento válido vai compartilhar.

    interface IMeioDePagamento
    {
        void LevantarFundos(decimal Saldo);
    }
```
E alteramos o nosso método para lidar com um objeto do tipo IMeioDePagamento, que retorna fundos, que serão utilizados para pagar pela música. Agora toda vez que eu for criar um meio de pagamento novo, ele só precisa implementar a interface para poder ser aceito pelo método de gerar ordens de pagamento.
```csharp
    public class GeradorDeOrdensDePagamento
    {
        public void GerarOrdemDePagamento(int IdMusica, IMeioDePagamento meioDePagamento)
        {
            //geraOrdemDePagamento utilizando o fundo fornecido pelo meio de pagamento.
        }
    }
```
Perceba que estou fechado para modificações, como existe uma interface para meios de pagamento, qualquer um que queira utilzar o método, terá que estender esse comportamento e não modificar. Novamente, aberto para extensões e fechado para modificações, **esse é o princípio**.

##  LSP, princípio de substituição de Liskov.

LSP traz uma visão sobre o uso de herança, onde diz que um objeto filho só pode e deveria herdar do objeto base quando o objeto base pode ser substituível pelo seu subtipo criado em todas as situações. Por exemplo:

Um objeto carro, tem seus métodos e propriedades, se criarmos um objeto carro elétrico que herda de carro teriamos um erro, pois há coisas que um carro faz que um carro elétrico não faz, como ter embragem, mostrar o nível de gasolina. Essa é a idéia defendida no LSP, que só se deve herdar de um classe, quando eu puder substituir seu objeto pelo objeto que eu criar a partir dele.

##  ISP, princípio de segregação de interface.

ISP diz que nenhum cliente de uma interface deve ser obrigado a depender de um método ou propriedade que ele não usa. Se houver uma interface muito grande que força os clientes, você deve quebrar essa interface em outras interfaces contendo esses métodos e propriedades adicionais, e dar liberdade para uma classe escolher ou não implementar a interface. Por exemplo:

```csharp
    interface ILeitorDeArquivo
    {
        void LerArquivo(string nomeDoArquivo);
    }
```
Digamos que várias classes clientes estão felizes implementando sua interface para ler arquivos, mas um belo dia os desenvolvedores decidiram adicionar um novo método **EnviarArquivoPorEmail()** na interface.

```csharp
    interface ILeitorDeArquivo
    {
        void LerArquivo(string nomeDoArquivo);

        void EnviarArquivoPorEmail(string nomeDoArquivo);
    }
```
Mas ao fazer isso eles receberam muitas reclamações pois havia clientes que já tinham seu meio de enviar por email, ou não queriam ter que implementar esse método. Como os desenvolvedores resolveriam isso? 

```csharp
    interface ILeitorDeArquivoV1
    {
        void LerArquivo(string nomeDoArquivo);

        void EnviarArquivoPorEmail(string nomeDoArquivo);
    }
```
Ao criar uma nova interface, nossas classes clientes que não querem ter que implementar o método de enviar o arquivo por email, podem manter a antiga interface, e as que desejam atualizar, podem implementar a nova interface. Viu, desse modo não forçamos nenhuma classe cliente a implementar métodos que não utiliza e declarar propriedades que não precisa, e não violamos o princípio do **ISP**.

## DIP, o princípio de inversão de dependência.

DIP, diz que você não deve depender de uma implementação concreta da classe e sim de sua abstração, por exemplo: Temos a classe abstrata **Humano**, e seus subtipos **Homem** e **Mulher**, se tivessemos uma classe **Hospital** que tem um método **TirarSangue()** que precisa de um humano para ser realizado, ele deve depender da abstração em si, do tipo **Humano**, e não das suas implementações concretas que no caso são **Homem** e **Mulher**, pois se **Homem** ou **Mulher** sofrem uma alteração, o fato de meu método depender dessa implementação pode causar impacto na sua classe, depender da abstração evita o alto acoplamento e diminuiu o risco de falhas por alteração. 


Quando eu tiver um caso de ter uma depedência onde está sendo tratada dentro da minha classe, mas isso quebra o princípio de **OCP** ou de **SRP**, entra a inversão de controle e de injeção de dependência,por exemplo: Se eu tenho um objeto que está sendo instânciado na minha classe.
```csharp
 public class MinhaClasse
 {
    Objeto obj = new Objeto;

    obj.FacaAlgo()
 }
```
 qualquer alteração que ele sofra, que eu tenha que alterar na classe e o ato de instânciar esse objeto dá uma responsabilidade a minha classe que não deveria ser dela, o que tem que ocorrer é eu delegar a instânciação desse objeto a outra classe através da inversão de controle,
 ```csharp
 public class MinhaOutraClasse
 {
    Objeto obj = new Objeto;
 }
 ```
  mas como eu dependo desse objeto, eu recebo ele através da injeção de dependência 
 ```csharp
 public class MinhaClasse
 {
    Objeto Obj;

    public MinhaClasse(Objeto obj)
    {
        this.obj = obj;
    }
 }
 ```

Espero que isso te ajude a compreender um pouco mais sobre o que é o SOLID e o quão útil ele é.

Obrigado a todos os leitores desse artigo.