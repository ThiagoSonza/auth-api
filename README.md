# Auth.API

**Auth.API** é uma Web API desenvolvida em .NET 8 com o propósito de centralizar os processos de **autenticação** e **autorização** das aplicações da companhia.  
Ela atua como um ponto único de controle de identidade, garantindo segurança, consistência e facilidade de integração entre os diversos serviços internos.

Este projeto foi idealizado para atender aos requisitos de escalabilidade, modularidade e modernização da arquitetura, adotando práticas contemporâneas como uso de Minimal APIs e versionamento nativo de endpoints.

## ✨ Principais Características

- 🔹 **Minimal APIs**: estrutura baseada em minimal endpoints para uma implementação mais enxuta, direta e de fácil manutenção.
- 🔹 **Versionamento de API**: suporte nativo ao versionamento via cabeçalhos HTTP, permitindo a evolução contínua da API sem impacto direto nos consumidores.
- 🔹 **Pacotes reutilizáveis**:
  - [`Thiagosza.RabbitMq.Core`](https://www.nuget.org/packages/Thiagosza.RabbitMq.Core): abstração para integração com RabbitMQ, facilitando a publicação e consumo de eventos.
  - [`Thiagosza.Mediator.Core`](https://www.nuget.org/packages/Thiagosza.Mediator.Core): encapsula a lógica de orquestração de comandos e queries com base no padrão CQRS usando MediatR.
- 🔹 **Autenticação baseada em JWT**: emissão de tokens de acesso com suporte a refresh tokens e validações robustas.
- 🔹 **Autorização via claims e roles**: controle granular de permissões com base no perfil do usuário e regras de negócio específicas.
- 🔹 **Modularização e injeção de dependência limpa**: organização por domínios e responsabilidades, promovendo alta coesão e baixo acoplamento.
- 🔹 **Telemetria integrada**: geração e exposição de logs, métricas e traces distribuídos para observabilidade completa da aplicação, permitindo análise de performance, rastreamento de chamadas e diagnóstico de falhas.

---
