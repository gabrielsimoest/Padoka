-- ============================================
-- SCRIPT PARA POPULAR CARDÁPIO - PADARIA PADOKA
-- Barretos - SP
-- ============================================

-- Limpar dados existentes (opcional - descomente se necessário)
-- DELETE FROM "ItensCardapio";
-- DELETE FROM "Categorias";

-- ============================================
-- CATEGORIAS
-- ============================================
INSERT INTO "Categorias" ("Nome", "Descricao", "Ordem", "Ativo", "CriadoEm") VALUES
('Bebida Quente', 'Cafés, chocolates e outras bebidas quentes', 1, true, NOW()),
('Bebida Gelada', 'Refrigerantes, sucos e bebidas geladas', 2, true, NOW()),
('Lanches', 'Sanduíches e lanches diversos', 3, true, NOW()),
('Adicionais', 'Acompanhamentos e adicionais para lanches', 4, true, NOW()),
('Omeletes', 'Omeletes variados', 5, true, NOW()),
('Lanches Naturais', 'Opções saudáveis e naturais', 6, true, NOW()),
('Salgados', 'Salgados assados e fritos', 7, true, NOW()),
('Sucos', 'Sucos naturais', 8, true, NOW()),
('Doces', 'Sobremesas e doces variados', 9, true, NOW());

-- ============================================
-- ITENS DO CARDÁPIO
-- ============================================

-- BEBIDAS QUENTES (CategoriaId = 1)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Café Expresso', 'Café expresso tradicional', 'Café expresso feito na hora com grãos selecionados', 'Café', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Café com Leite Pequeno', 'Café com leite tamanho pequeno', 'Café expresso com leite vaporizado', 'Café, Leite', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Café com Leite Médio', 'Café com leite tamanho médio', 'Café expresso com leite vaporizado', 'Café, Leite', 7.50, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Café com Leite Grande', 'Café com leite tamanho grande', 'Café expresso com leite vaporizado', 'Café, Leite', 9.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Cappuccino Pequeno', 'Cappuccino tamanho pequeno', 'Café expresso com leite vaporizado e espuma de leite', 'Café, Leite, Espuma de leite, Canela', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Cappuccino Médio', 'Cappuccino tamanho médio', 'Café expresso com leite vaporizado e espuma de leite', 'Café, Leite, Espuma de leite, Canela', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Cappuccino Grande', 'Cappuccino tamanho grande', 'Café expresso com leite vaporizado e espuma de leite', 'Café, Leite, Espuma de leite, Canela', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Chocolate Quente Pequeno', 'Chocolate quente tamanho pequeno', 'Chocolate cremoso feito com leite', 'Chocolate, Leite', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Chocolate Quente Médio', 'Chocolate quente tamanho médio', 'Chocolate cremoso feito com leite', 'Chocolate, Leite', 9.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Chocolate Quente Grande', 'Chocolate quente tamanho grande', 'Chocolate cremoso feito com leite', 'Chocolate, Leite', 11.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Chá', 'Chá natural', 'Chá de ervas naturais', 'Chá', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Leite Quente', 'Leite puro aquecido', 'Leite fresco aquecido', 'Leite', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente')),
('Toddy Quente', 'Achocolatado Toddy quente', 'Bebida achocolatada quente', 'Toddy, Leite', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Quente'));

-- BEBIDAS GELADAS (CategoriaId = 2)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Coca-Cola Lata', 'Refrigerante Coca-Cola 350ml', 'Coca-Cola gelada em lata', 'Refrigerante', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Coca-Cola 600ml', 'Refrigerante Coca-Cola 600ml', 'Coca-Cola gelada garrafa 600ml', 'Refrigerante', 9.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Guaraná Antarctica Lata', 'Refrigerante Guaraná 350ml', 'Guaraná Antarctica gelado em lata', 'Refrigerante', 5.50, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Guaraná Antarctica 600ml', 'Refrigerante Guaraná 600ml', 'Guaraná Antarctica gelado garrafa 600ml', 'Refrigerante', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Suco Del Valle Lata', 'Suco Del Valle diversos sabores', 'Suco Del Valle gelado em lata', 'Suco', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Água Mineral', 'Água mineral sem gás 500ml', 'Água mineral gelada', 'Água', 3.50, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Água com Gás', 'Água mineral com gás 500ml', 'Água mineral com gás gelada', 'Água gaseificada', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Toddy Gelado', 'Achocolatado Toddy gelado', 'Bebida achocolatada gelada', 'Toddy, Leite, Gelo', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Leite Gelado', 'Leite puro gelado', 'Leite fresco gelado', 'Leite, Gelo', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada')),
('Café Gelado', 'Café expresso gelado', 'Café expresso com gelo', 'Café, Gelo', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Bebida Gelada'));

-- LANCHES (CategoriaId = 3)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Misto Quente', 'Pão de forma com presunto e queijo', 'Clássico misto quente na chapa', 'Pão de forma, Presunto, Queijo', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Misto Quente Duplo', 'Misto quente com dobro de recheio', 'Misto quente com presunto e queijo em dobro', 'Pão de forma, Presunto (2x), Queijo (2x)', 16.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Queijo Quente', 'Pão de forma com queijo derretido', 'Pão de forma com queijo na chapa', 'Pão de forma, Queijo', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Pão com Manteiga', 'Pão francês com manteiga na chapa', 'Pão francês quentinho com manteiga', 'Pão francês, Manteiga', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Pão na Chapa', 'Pão francês tostado na chapa', 'Pão francês crocante', 'Pão francês', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('X-Burguer', 'Hambúrguer com queijo', 'Hambúrguer bovino com queijo, alface e tomate', 'Pão, Hambúrguer, Queijo, Alface, Tomate', 18.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('X-Salada', 'Hambúrguer com queijo e salada', 'Hambúrguer bovino completo com salada', 'Pão, Hambúrguer, Queijo, Alface, Tomate, Cebola', 20.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('X-Bacon', 'Hambúrguer com queijo e bacon', 'Hambúrguer bovino com queijo e bacon crocante', 'Pão, Hambúrguer, Queijo, Bacon, Alface, Tomate', 22.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('X-Egg', 'Hambúrguer com queijo e ovo', 'Hambúrguer bovino com queijo e ovo frito', 'Pão, Hambúrguer, Queijo, Ovo, Alface, Tomate', 21.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('X-Tudo', 'Hambúrguer completo', 'Hambúrguer bovino com todos os ingredientes', 'Pão, Hambúrguer, Queijo, Bacon, Ovo, Presunto, Alface, Tomate', 28.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Bauru Simples', 'Sanduíche Bauru tradicional', 'Pão francês com presunto e queijo derretido', 'Pão francês, Presunto, Queijo, Tomate', 14.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Bauru Especial', 'Sanduíche Bauru completo', 'Pão francês com presunto, queijo, ovo e bacon', 'Pão francês, Presunto, Queijo, Ovo, Bacon, Tomate', 18.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Americano', 'Sanduíche americano', 'Pão com hambúrguer, presunto e queijo', 'Pão, Hambúrguer, Presunto, Queijo', 16.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Cachorro Quente', 'Hot dog tradicional', 'Pão de hot dog com salsicha e molhos', 'Pão de hot dog, Salsicha, Molho, Batata palha', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches')),
('Cachorro Quente Especial', 'Hot dog completo', 'Pão de hot dog com salsicha, queijo, bacon e purê', 'Pão de hot dog, Salsicha, Queijo, Bacon, Purê, Molho', 16.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches'));

-- ADICIONAIS (CategoriaId = 4)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Adicional de Queijo', 'Fatia extra de queijo', 'Queijo mussarela extra', 'Queijo', 3.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Presunto', 'Fatia extra de presunto', 'Presunto extra', 'Presunto', 3.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Bacon', 'Porção extra de bacon', 'Bacon crocante extra', 'Bacon', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Ovo', 'Ovo frito extra', 'Ovo frito extra', 'Ovo', 3.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Hambúrguer', 'Hambúrguer extra', 'Hambúrguer bovino extra', 'Hambúrguer', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Catupiry', 'Porção extra de catupiry', 'Catupiry cremoso extra', 'Catupiry', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais')),
('Adicional de Cheddar', 'Porção extra de cheddar', 'Queijo cheddar extra', 'Cheddar', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Adicionais'));

-- OMELETES (CategoriaId = 5)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Omelete Simples', 'Omelete básico', 'Omelete feito com ovos frescos', 'Ovos, Sal, Óleo', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete com Queijo', 'Omelete recheado com queijo', 'Omelete com queijo mussarela derretido', 'Ovos, Queijo', 13.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete com Presunto', 'Omelete recheado com presunto', 'Omelete com presunto picado', 'Ovos, Presunto', 13.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete Misto', 'Omelete com presunto e queijo', 'Omelete recheado com presunto e queijo', 'Ovos, Presunto, Queijo', 15.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete com Bacon', 'Omelete recheado com bacon', 'Omelete com bacon crocante', 'Ovos, Bacon', 15.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete Completo', 'Omelete com todos ingredientes', 'Omelete recheado com presunto, queijo e bacon', 'Ovos, Presunto, Queijo, Bacon', 18.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes')),
('Omelete com Legumes', 'Omelete com vegetais', 'Omelete recheado com tomate, cebola e pimentão', 'Ovos, Tomate, Cebola, Pimentão', 14.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Omeletes'));

-- LANCHES NATURAIS (CategoriaId = 6)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Sanduíche Natural de Frango', 'Sanduíche saudável de frango', 'Pão integral com frango desfiado, cenoura e milho', 'Pão integral, Frango desfiado, Cenoura, Milho, Maionese light', 14.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Sanduíche Natural de Atum', 'Sanduíche saudável de atum', 'Pão integral com atum, cenoura e milho', 'Pão integral, Atum, Cenoura, Milho, Maionese light', 15.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Sanduíche Natural de Peito de Peru', 'Sanduíche saudável de peito de peru', 'Pão integral com peito de peru, queijo branco e tomate', 'Pão integral, Peito de peru, Queijo branco, Tomate, Alface', 16.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Wrap de Frango', 'Wrap saudável de frango', 'Tortilha integral com frango grelhado e salada', 'Tortilha integral, Frango grelhado, Alface, Tomate, Cenoura', 18.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Salada de Frutas', 'Mix de frutas frescas', 'Salada com frutas da estação', 'Frutas variadas da estação', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Açaí 300ml', 'Açaí na tigela pequeno', 'Açaí batido com banana e granola', 'Açaí, Banana, Granola, Leite condensado', 15.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais')),
('Açaí 500ml', 'Açaí na tigela médio', 'Açaí batido com banana, granola e frutas', 'Açaí, Banana, Granola, Morango, Leite condensado', 22.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Lanches Naturais'));

-- SALGADOS (CategoriaId = 7)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Coxinha', 'Coxinha de frango', 'Coxinha cremosa de frango', 'Massa, Frango desfiado, Catupiry', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Esfiha de Carne', 'Esfiha aberta de carne', 'Esfiha com carne temperada', 'Massa, Carne moída, Cebola, Tomate', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Esfiha de Queijo', 'Esfiha aberta de queijo', 'Esfiha com queijo mussarela', 'Massa, Queijo mussarela', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Esfiha de Frango', 'Esfiha aberta de frango', 'Esfiha com frango desfiado', 'Massa, Frango desfiado, Catupiry', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Pastel de Carne', 'Pastel frito de carne', 'Pastel crocante recheado com carne', 'Massa de pastel, Carne moída', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Pastel de Queijo', 'Pastel frito de queijo', 'Pastel crocante recheado com queijo', 'Massa de pastel, Queijo', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Pastel de Frango', 'Pastel frito de frango', 'Pastel crocante recheado com frango', 'Massa de pastel, Frango desfiado, Catupiry', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Pastel de Pizza', 'Pastel frito sabor pizza', 'Pastel com queijo, presunto e tomate', 'Massa de pastel, Queijo, Presunto, Tomate, Orégano', 9.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Empada de Frango', 'Empada de frango', 'Empada recheada com frango cremoso', 'Massa, Frango desfiado, Requeijão', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Empada de Palmito', 'Empada de palmito', 'Empada recheada com palmito', 'Massa, Palmito, Requeijão', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Pão de Queijo', 'Pão de queijo tradicional', 'Pão de queijo mineiro', 'Polvilho, Queijo, Ovo, Leite', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Enroladinho de Salsicha', 'Salsicha enrolada na massa', 'Massa folhada com salsicha', 'Massa folhada, Salsicha', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Croissant Presunto e Queijo', 'Croissant recheado', 'Croissant folhado com presunto e queijo', 'Massa folhada, Presunto, Queijo', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Kibe', 'Kibe frito', 'Kibe tradicional de carne', 'Trigo, Carne moída, Hortelã', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados')),
('Bolinho de Bacalhau', 'Bolinho de bacalhau frito', 'Bolinho cremoso de bacalhau', 'Bacalhau, Batata, Salsinha', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Salgados'));

-- SUCOS (CategoriaId = 8)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Suco de Laranja 300ml', 'Suco natural de laranja', 'Suco de laranja espremido na hora', 'Laranja', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Laranja 500ml', 'Suco natural de laranja grande', 'Suco de laranja espremido na hora', 'Laranja', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Limão 300ml', 'Suco natural de limão', 'Limonada fresca', 'Limão, Açúcar, Água', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Limão 500ml', 'Suco natural de limão grande', 'Limonada fresca', 'Limão, Açúcar, Água', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Abacaxi 300ml', 'Suco natural de abacaxi', 'Suco de abacaxi batido', 'Abacaxi, Água', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Abacaxi 500ml', 'Suco natural de abacaxi grande', 'Suco de abacaxi batido', 'Abacaxi, Água', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Maracujá 300ml', 'Suco natural de maracujá', 'Suco de maracujá batido', 'Maracujá, Água', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Maracujá 500ml', 'Suco natural de maracujá grande', 'Suco de maracujá batido', 'Maracujá, Água', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Morango 300ml', 'Suco natural de morango', 'Suco de morango com leite', 'Morango, Leite', 9.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Morango 500ml', 'Suco natural de morango grande', 'Suco de morango com leite', 'Morango, Leite', 14.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Vitamina de Banana 300ml', 'Vitamina de banana com leite', 'Vitamina cremosa de banana', 'Banana, Leite', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Vitamina de Banana 500ml', 'Vitamina de banana com leite grande', 'Vitamina cremosa de banana', 'Banana, Leite', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Melancia 300ml', 'Suco natural de melancia', 'Suco refrescante de melancia', 'Melancia', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco de Melancia 500ml', 'Suco natural de melancia grande', 'Suco refrescante de melancia', 'Melancia', 12.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos')),
('Suco Misto', 'Combinação de frutas', 'Suco com mix de frutas à escolha', 'Frutas variadas', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Sucos'));

-- DOCES (CategoriaId = 9)
INSERT INTO "ItensCardapio" ("Nome", "DescricaoResumida", "DescricaoCompleta", "Ingredientes", "Preco", "ImagemUrl", "Ativo", "CriadoEm", "CategoriaId") VALUES
('Pão Doce', 'Pão doce tradicional', 'Pão macio com açúcar', 'Farinha, Açúcar, Leite, Ovo', 4.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Pão de Mel', 'Pão de mel com chocolate', 'Pão de mel coberto com chocolate', 'Mel, Chocolate, Especiarias', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Sonho', 'Sonho recheado', 'Sonho macio recheado com creme', 'Massa, Creme, Açúcar', 6.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Croissant Chocolate', 'Croissant com chocolate', 'Croissant folhado com recheio de chocolate', 'Massa folhada, Chocolate', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Fatia de Bolo', 'Bolo do dia', 'Fatia de bolo caseiro do dia', 'Farinha, Açúcar, Ovos, Leite', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Fatia de Bolo de Chocolate', 'Bolo de chocolate', 'Fatia de bolo de chocolate com cobertura', 'Farinha, Chocolate, Açúcar, Ovos', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Fatia de Bolo de Cenoura', 'Bolo de cenoura', 'Fatia de bolo de cenoura com calda de chocolate', 'Cenoura, Farinha, Chocolate', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Pudim', 'Pudim de leite', 'Pudim de leite condensado', 'Leite condensado, Leite, Ovos, Caramelo', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Brigadeiro', 'Brigadeiro tradicional', 'Brigadeiro de chocolate com granulado', 'Chocolate, Leite condensado, Manteiga', 3.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Beijinho', 'Beijinho de coco', 'Docinho de coco tradicional', 'Coco, Leite condensado, Manteiga', 3.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Quindim', 'Quindim tradicional', 'Doce de coco e gema', 'Coco, Gema de ovo, Açúcar', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Trufa', 'Trufa de chocolate', 'Trufa cremosa de chocolate', 'Chocolate, Creme de leite', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Churros', 'Churros com recheio', 'Churros crocante com doce de leite', 'Massa de churros, Doce de leite, Açúcar e canela', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Torta de Limão', 'Fatia de torta de limão', 'Torta de limão com merengue', 'Limão, Leite condensado, Merengue', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Torta de Maracujá', 'Fatia de torta de maracujá', 'Torta de maracujá cremosa', 'Maracujá, Leite condensado, Creme de leite', 10.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Mil Folhas', 'Doce mil folhas', 'Massa folhada com creme', 'Massa folhada, Creme confeiteiro', 8.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Carolina', 'Carolina de chocolate', 'Massa choux com chocolate', 'Massa choux, Creme, Chocolate', 5.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces')),
('Bomba de Chocolate', 'Bomba recheada', 'Bomba com recheio de chocolate', 'Massa, Chocolate, Creme', 7.00, NULL, true, NOW(), (SELECT "Id" FROM "Categorias" WHERE "Nome" = 'Doces'));

-- ============================================
-- VERIFICAÇÃO
-- ============================================
SELECT 'Categorias inseridas:' AS "Info", COUNT(*) AS "Total" FROM "Categorias";
SELECT 'Itens inseridos:' AS "Info", COUNT(*) AS "Total" FROM "ItensCardapio";

-- Resumo por categoria
SELECT c."Nome" AS "Categoria", COUNT(i."Id") AS "Itens"
FROM "Categorias" c
LEFT JOIN "ItensCardapio" i ON c."Id" = i."CategoriaId"
GROUP BY c."Nome", c."Ordem"
ORDER BY c."Ordem";
