// DESAFIO: Sistema de Aprovação de Despesas Corporativas
// PROBLEMA: Uma empresa precisa processar pedidos de reembolso com diferentes níveis de aprovação
// baseados no valor. O código atual usa condicionais gigantes e está difícil de manter
// quando novos níveis de aprovação são adicionados

using System;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de RH que processa reembolsos de despesas
    // Cada nível gerencial tem limite de aprovação diferente
    
    public class ExpenseRequest
    {
        public string EmployeeName { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public string Department { get; set; }

        public ExpenseRequest(string employeeName, decimal amount, string purpose, string department)
        {
            EmployeeName = employeeName;
            Amount = amount;
            Purpose = purpose;
            Department = department;
        }
    }

    // Problema: Classe monolítica com lógica condicional complexa
    public class ExpenseApprovalSystem
    {
        public void ProcessExpense(ExpenseRequest request)
        {
            Console.WriteLine($"\n=== Processando Despesa ===");
            Console.WriteLine($"Funcionário: {request.EmployeeName}");
            Console.WriteLine($"Valor: R$ {request.Amount:N2}");
            Console.WriteLine($"Propósito: {request.Purpose}");
            Console.WriteLine($"Departamento: {request.Department}\n");

            // Problema 1: Lógica condicional aninhada e complexa
            if (request.Amount <= 100)
            {
                // Supervisor pode aprovar
                Console.WriteLine("[Supervisor] Analisando pedido...");
                
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount))
                {
                    Console.WriteLine($"✅ [Supervisor] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Supervisor", request);
                }
                else
                {
                    Console.WriteLine($"❌ [Supervisor] Despesa REJEITADA - Documentação inválida");
                }
            }
            else if (request.Amount <= 500)
            {
                // Gerente pode aprovar
                Console.WriteLine("[Supervisor] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[Gerente] Analisando pedido...");
                
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) && 
                    CheckPolicy(request))
                {
                    Console.WriteLine($"✅ [Gerente] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Gerente", request);
                }
                else
                {
                    Console.WriteLine($"❌ [Gerente] Despesa REJEITADA");
                }
            }
            else if (request.Amount <= 5000)
            {
                // Diretor pode aprovar
                Console.WriteLine("[Supervisor] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[Gerente] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[Diretor] Analisando pedido...");
                
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) && 
                    CheckPolicy(request) && CheckStrategicAlignment(request))
                {
                    Console.WriteLine($"✅ [Diretor] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Diretor", request);
                }
                else
                {
                    Console.WriteLine($"❌ [Diretor] Despesa REJEITADA");
                }
            }
            else
            {
                // CEO deve aprovar
                Console.WriteLine("[Supervisor] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[Gerente] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[Diretor] Valor acima do meu limite, encaminhando...");
                Console.WriteLine("[CEO] Analisando pedido...");
                
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) && 
                    CheckPolicy(request) && CheckStrategicAlignment(request) && CheckBoardApproval(request))
                {
                    Console.WriteLine($"✅ [CEO] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("CEO", request);
                }
                else
                {
                    Console.WriteLine($"❌ [CEO] Despesa REJEITADA");
                }
            }

            // Problema 2: Adicionar novo nível de aprovação requer modificar toda esta estrutura
            // Problema 3: Lógica de encaminhamento está duplicada
            // Problema 4: Não é fácil mudar a ordem ou pular níveis
        }

        private bool ValidateReceipt(ExpenseRequest request)
        {
            Console.WriteLine("  → Validando nota fiscal...");
            return true; // Simulação
        }

        private bool CheckBudget(string department, decimal amount)
        {
            Console.WriteLine($"  → Verificando orçamento do departamento {department}...");
            return true; // Simulação
        }

        private bool CheckPolicy(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando conformidade com política...");
            return true; // Simulação
        }

        private bool CheckStrategicAlignment(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando alinhamento estratégico...");
            return true; // Simulação
        }

        private bool CheckBoardApproval(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando aprovação do conselho...");
            return true; // Simulação
        }

        private void RegisterApproval(string approver, ExpenseRequest request)
        {
            Console.WriteLine($"  → Registrando aprovação por {approver}...");
        }
    }

    // Alternativa problemática: Switch case
    public class ExpenseApprovalSystemV2
    {
        public void ProcessExpense(ExpenseRequest request)
        {
            var approvalLevel = DetermineApprovalLevel(request.Amount);
            
            // Problema: Ainda requer modificação para adicionar novos níveis
            switch (approvalLevel)
            {
                case "Supervisor":
                    ProcessBySupervisor(request);
                    break;
                case "Gerente":
                    ProcessBySupervisor(request); // Passa por todos
                    ProcessByManager(request);
                    break;
                case "Diretor":
                    ProcessBySupervisor(request);
                    ProcessByManager(request);
                    ProcessByDirector(request);
                    break;
                case "CEO":
                    ProcessBySupervisor(request);
                    ProcessByManager(request);
                    ProcessByDirector(request);
                    ProcessByCEO(request);
                    break;
            }
        }

        private string DetermineApprovalLevel(decimal amount)
        {
            if (amount <= 100) return "Supervisor";
            if (amount <= 500) return "Gerente";
            if (amount <= 5000) return "Diretor";
            return "CEO";
        }

        private void ProcessBySupervisor(ExpenseRequest request) { }
        private void ProcessByManager(ExpenseRequest request) { }
        private void ProcessByDirector(ExpenseRequest request) { }
        private void ProcessByCEO(ExpenseRequest request) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Aprovação de Despesas ===");

            var system = new ExpenseApprovalSystem();

            // Teste com diferentes valores
            var expense1 = new ExpenseRequest("João Silva", 50.00m, "Material de escritório", "TI");
            system.ProcessExpense(expense1);

            var expense2 = new ExpenseRequest("Maria Santos", 350.00m, "Curso de capacitação", "RH");
            system.ProcessExpense(expense2);

            var expense3 = new ExpenseRequest("Pedro Oliveira", 2500.00m, "Notebook", "TI");
            system.ProcessExpense(expense3);

            var expense4 = new ExpenseRequest("Ana Costa", 15000.00m, "Servidor para datacenter", "TI");
            system.ProcessExpense(expense4);

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Lógica condicional profundamente aninhada e complexa");
            Console.WriteLine("✗ Código duplicado para encaminhamento entre níveis");
            Console.WriteLine("✗ Adicionar novo nível requer modificar toda estrutura");
            Console.WriteLine("✗ Difícil alterar ordem ou critérios de aprovação");
            Console.WriteLine("✗ Não é possível compor diferentes cadeias dinamicamente");
            Console.WriteLine("✗ Testabilidade comprometida - difícil testar níveis isoladamente");
            Console.WriteLine("✗ Viola Single Responsibility: uma classe faz tudo");

            // Perguntas para reflexão:
            // - Como desacoplar os níveis de aprovação?
            // - Como permitir que cada nível decida se processa ou encaminha?
            // - Como facilitar adição/remoção de níveis sem modificar código existente?
            // - Como criar diferentes cadeias de aprovação dinamicamente?
        }
    }
}
