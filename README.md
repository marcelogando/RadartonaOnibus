# RadartonaOnibus


Fizemos uma solução para ônibus, pensando em uma dor real das empresas: como monitorar as frotas de ônibus em tempo real, e conseguir prever acidentes?
Pois é. Com essa solução, é possível monitorar através de um mapa quais ônibus estão em uma zona em que foi reportado algum acidente.
Vale ressaltar que são acidentes monitorados pelo waze e pelos radares em tempo real. Logo, se o ônibus ficar muito tempo parado na zona de acidente, é porque ele provavelmente precisa de ajuda! :)




Participantes:
Marcel Carvalho Ogando
Lucas Simões da Silva
Anna Flávia Castro
Willian Rodrigues Chan


Análises:
- Detectamos que a API do olho vivo trouxe em torno de 1100 ônibus em tempo real em São Paulo
- Detectamos 7 ônibus presentes em locais de acidente no Waze, o que permite pensar de que eles podem ter se envolvido em algum

Métodos utilizados:
- Login: Realiza o login na API do olho vivo e retorna o token utilizado para ela
- Index: Realiza um POST na API do olho vivo e recebe os dados dos ônibus. Também faz o match da latitude e longitude dos ônibus
- GetAlerta: Compara os dados da API com os dados de acidentes e imprevistos em radares e waze
