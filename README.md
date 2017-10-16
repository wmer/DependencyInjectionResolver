﻿# DependencyInjectionResolver
Biblioteca para Injeção de dependências
## Compatibilidade
.net Standard 1.6
## Dependências
System.ValueTuple  
Reflection.Optimizations  
Microsoft.Extensions.DependencyModel  
# Exemplos  
var class = new Class{ new Dependency1(), new Dependency2(), new Dependency3() };  
Com dependencyInjectionResolver:  
Quando não há possibilidades de ambiguidades, classes distintas que implementam a mesa interface podem ser feitas simplesmente assim:  
Class class = new DependencyInjection().Resolve<Class>();  
Já em casos onde é preciso especificar a dependência, pode ser feito assim:  
IClass class = new DependencyInjection()  
                .BindingTypes<IDependency1, Class1>()  
                .BindingTypes<IDependency2, Class4>()   
                                                .Resolve<IClass>();  
  
>.Resolve() sempre retornará a primeira instância do objeto, caso seja preciso uma instância diferente é preciso especificar com:  
>  .Resolve(InstanceOptions.DiferentInstances).  
> Todos os métodos possuem uma versão não-genérica, flexibilizando seu uso, porém, o cast é necessário:
> Class class = new DependencyInjection().Resolve(typeof(Class)) as Class;  
> Nos próximos exemplos irei alternar as duas formas.
### Definindo assinatura de classe
É possível definir a assinatura da classe e/ou de suas dependências:
IClass class = new DependencyInjection()  
  .BindingTypes<IClass, Class>()  
  .DefineConstructorSignature<Class>(typeof(IDependency1), typeof(IDependency2))  
  .Resolve<IClass>();  
  Não genérico:
  IClass class = new DependencyInjection()  
  .DefineConstructorSignature(typeof(Class), typeof(IDependency1), typeof(IDependency2))  
  .Resolve<IClass>();  
  >O primeiro parâmetro especifica a classe ao qual se deseja definir a assinatura.  
  >Por padrão, a classe é instanciada a partir do construtor com o maior número de dependências. 
  ### Passando objetos para o construtor
  Para passar objetos previamente instanciados ao construtor é necessário especificar a classe, a posição ou parâmetro do construtor, dessa forma:  
  Class2 class2 = new Class2();  
  Class class = new DependencyInjection()  
  .DefineDependency<Class>(1, class2)  
  .Resolve(typeof(Class)) as Class;  
  Com o nome do parâmetro:  
   Class2 class2 = new Class2();  
  Class class = new DependencyInjection()  
  .DefineDependency(typeof(Class), "class2", class2)  
  .Resolve<Class>();  