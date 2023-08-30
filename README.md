EN VERSION HERE: https://github.com/DavBraga/TabletsUnityFB/blob/main/englishREADME.MD

# Catálogo de Dispositivos

Um aplicativo CRUD para dispositivos móveis usando Unity e Firebase.

## Descrição

Este projeto é um aplicativo para dispositivos móveis desenvolvido com Unity e Firebase.Pensando na necessidade de uma instituição educacional de fazer o controle de dispostivos móveis em seu acervo, o aplicativo foi modelado afim de  permitir registrar dispositivos em uma base de dados e adicionar observações sobre cada dispositivo. Além disso, o aplicativo tem a funcionalidade de leitura de QR code e código de barras para capturar números de patrimônio e IMEI dos dispositivos. O aplicativo também oferece recursos de pesquisa, filtros e edição de dados já inseridos no banco de dados. A autenticação básica com o Firebase é implementada para garantir que usuários possam acessar e modificar as informações dos dispositivos.

## Funcionalidades

- Registro de dispositivos móveis: Os usuários podem adicionar informações sobre um dispositivo móvel,como paradeiro,status, e número de série, à base de dados.

- Observações sobre dispositivos: Os usuários podem adicionar observações ou notas específicas sobre cada dispositivo registrado com timestamp do momento em que a observação foi adicionada ao banco.

- Leitura de QR code e código de barras: O aplicativo possui a capacidade de fazer a leitura de QR codes e códigos de barras para capturar automaticamente os números de patrimônio e IMEI dos dispositivos.

- Pesquisa e filtros: Os usuários podem realizar pesquisas e aplicar filtros para encontrar dispositivos específicos com base em critérios como patrimônio, IMEI, status e paradeiro.

- Edição e exclusão de dados: Os usuários podem editar as informações já inseridas no banco de dados, permitindo a atualização de detalhes dos dispositivos registrados.

- Autenticação básica com Firebase: A autenticação de usuário é implementada para garantir que apenas usuários autorizados possam acessar seus próprios dados. O modo visitante permite acessar dados abertos ao público para teste de funcionalidades.

- Armazenamento Utilizando o Firestore: Todas as informações dos dispositivos e observações são armazenadas usando o Firebase Firestore.

## Requisitos do Sistema

- Dispositivo móvel Android 5.1 ou superior(Precisa de testes).

## Instalação e Configuração

1. Faça o download do repositório do projeto.

2. Abra o projeto no Unity.( Versão Unity: 2022.3.3f1 )

3. Configuração do Firebase:
   - Crie um projeto no Firebase Console (https://console.firebase.google.com/) e obtenha as credenciais do projeto.
   - Adicione as credenciais do projeto no Unity seguindo as instruções do Firebase SDK.
   - Habilite o Firebase Firestore para armazenamento dos dados.
   - Configure a autenticação básica com o Firebase Authentication.

4. Compilação do aplicativo:
   - Configure as plataformas de destino desejadas (Android, iOS) nas configurações do projeto no Unity.
   - Compile e crie o pacote de instalação do aplicativo para a plataforma escolhida.

5. Instale o aplicativo no dispositivo móvel.
