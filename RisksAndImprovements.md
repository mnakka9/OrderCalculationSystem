# Risks

1. **SQLite for Database**: The use of SQLite poses a high risk due to scalability issues.
2. **Hardcoded Tax Rules**: This presents a medium risk as it affects maintainability.
3. **Tax Calculation in Code**: Conducting tax calculations within the codebase introduces a medium risk, potentially leading to performance issues.

## Improvements

1. **Database Migration**: Transition to a more robust database system such as SQL Server or PostgreSQL to enhance performance and prepare for production environments.
2. **Logging and Monitoring**: Implement comprehensive logging and monitoring solutions to improve system observability and reliability.
3. **Decoupling Tax Rules**: Utilize a rule engine such as Microsoft Rule Engine to decouple tax rules from the codebase, thereby improving flexibility and maintainability.
4. **Distributed Caching**: Integrate a distributed cache solution like Redis to manage caching efficiently, which will significantly enhance performance.
