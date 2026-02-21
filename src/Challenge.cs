using System;

namespace DesignPatternChallenge
{
    // ============================
    // 1) Request
    // ============================
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

    // ============================
    // 2) Handler base
    // ============================
    public abstract class ApprovalHandler
    {
        protected ApprovalHandler _next;

        public ApprovalHandler SetNext(ApprovalHandler next)
        {
            _next = next;
            return next;
        }

        public void Handle(ExpenseRequest request)
        {
            Console.WriteLine($"\n=== Processando Despesa ===");
            Console.WriteLine($"Funcionário: {request.EmployeeName}");
            Console.WriteLine($"Valor: R$ {request.Amount:N2}");
            Console.WriteLine($"Propósito: {request.Purpose}");
            Console.WriteLine($"Departamento: {request.Department}\n");

            Process(request);
        }

        protected void PassToNext(ExpenseRequest request)
        {
            if (_next != null)
            {
                _next.Process(request);
            }
            else
            {
                Console.WriteLine("❌ Nenhum aprovador disponível para este valor. Pedido REJEITADO.");
            }
        }

        protected abstract void Process(ExpenseRequest request);

        // helpers “comuns” (poderiam ser serviços injetados em projeto real)
        protected bool ValidateReceipt(ExpenseRequest request)
        {
            Console.WriteLine("  → Validando nota fiscal...");
            return true;
        }

        protected bool CheckBudget(string department, decimal amount)
        {
            Console.WriteLine($"  → Verificando orçamento do departamento {department}...");
            return true;
        }

        protected bool CheckPolicy(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando conformidade com política...");
            return true;
        }

        protected bool CheckStrategicAlignment(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando alinhamento estratégico...");
            return true;
        }

        protected bool CheckBoardApproval(ExpenseRequest request)
        {
            Console.WriteLine("  → Verificando aprovação do conselho...");
            return true;
        }

        protected void RegisterApproval(string approver, ExpenseRequest request)
        {
            Console.WriteLine($"  → Registrando aprovação por {approver}...");
        }
    }

    // ============================
    // 3) Concrete Handlers
    // ============================
    public class SupervisorApproval : ApprovalHandler
    {
        private const decimal Limit = 100m;

        protected override void Process(ExpenseRequest request)
        {
            if (request.Amount <= Limit)
            {
                Console.WriteLine("[Supervisor] Analisando pedido...");
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount))
                {
                    Console.WriteLine($"✅ [Supervisor] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Supervisor", request);
                }
                else
                {
                    Console.WriteLine("❌ [Supervisor] Despesa REJEITADA");
                }
                return;
            }

            Console.WriteLine("[Supervisor] Valor acima do meu limite, encaminhando...");
            PassToNext(request);
        }
    }

    public class ManagerApproval : ApprovalHandler
    {
        private const decimal Limit = 500m;

        protected override void Process(ExpenseRequest request)
        {
            if (request.Amount <= Limit)
            {
                Console.WriteLine("[Gerente] Analisando pedido...");
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) && CheckPolicy(request))
                {
                    Console.WriteLine($"✅ [Gerente] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Gerente", request);
                }
                else
                {
                    Console.WriteLine("❌ [Gerente] Despesa REJEITADA");
                }
                return;
            }

            Console.WriteLine("[Gerente] Valor acima do meu limite, encaminhando...");
            PassToNext(request);
        }
    }

    public class DirectorApproval : ApprovalHandler
    {
        private const decimal Limit = 5000m;

        protected override void Process(ExpenseRequest request)
        {
            if (request.Amount <= Limit)
            {
                Console.WriteLine("[Diretor] Analisando pedido...");
                if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) &&
                    CheckPolicy(request) && CheckStrategicAlignment(request))
                {
                    Console.WriteLine($"✅ [Diretor] Despesa de R$ {request.Amount:N2} APROVADA");
                    RegisterApproval("Diretor", request);
                }
                else
                {
                    Console.WriteLine("❌ [Diretor] Despesa REJEITADA");
                }
                return;
            }

            Console.WriteLine("[Diretor] Valor acima do meu limite, encaminhando...");
            PassToNext(request);
        }
    }

    public class CEOApproval : ApprovalHandler
    {
        protected override void Process(ExpenseRequest request)
        {
            Console.WriteLine("[CEO] Analisando pedido...");
            if (ValidateReceipt(request) && CheckBudget(request.Department, request.Amount) &&
                CheckPolicy(request) && CheckStrategicAlignment(request) && CheckBoardApproval(request))
            {
                Console.WriteLine($"✅ [CEO] Despesa de R$ {request.Amount:N2} APROVADA");
                RegisterApproval("CEO", request);
            }
            else
            {
                Console.WriteLine("❌ [CEO] Despesa REJEITADA");
            }
        }
    }

    // ============================
    // 4) Demo
    // ============================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Aprovação de Despesas (Chain of Responsibility) ===");

            // Monta a cadeia dinamicamente
            var supervisor = new SupervisorApproval();
            var manager = new ManagerApproval();
            var director = new DirectorApproval();
            var ceo = new CEOApproval();

            supervisor.SetNext(manager).SetNext(director).SetNext(ceo);

            // Testes
            supervisor.Handle(new ExpenseRequest("João Silva", 50.00m, "Material de escritório", "TI"));
            supervisor.Handle(new ExpenseRequest("Maria Santos", 350.00m, "Curso de capacitação", "RH"));
            supervisor.Handle(new ExpenseRequest("Pedro Oliveira", 2500.00m, "Notebook", "TI"));
            supervisor.Handle(new ExpenseRequest("Ana Costa", 15000.00m, "Servidor para datacenter", "TI"));

            Console.WriteLine("\n✅ Benefícios:");
            Console.WriteLine("• Sem if/else gigante");
            Console.WriteLine("• Fácil adicionar/remover níveis");
            Console.WriteLine("• Cadeias diferentes podem ser montadas dinamicamente");
        }
    }
}