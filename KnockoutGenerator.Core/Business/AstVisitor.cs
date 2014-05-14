using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using KnockoutGenerator.Core.Models;
using System.Linq;
using System.Runtime.Serialization;

namespace KnockoutGenerator.Core.Business
{
    internal class AstVisitor : AbstractAstVisitor
    {
        public JsClass CurrentParent { get; set; }
        public JsFile Model { get; set; }

        public override object VisitCompilationUnit(CompilationUnit compilationUnit, object data)
        {
            Contract.Requires(compilationUnit != null);

            // Visit children (E.g. TypeDcelarion objects)
            compilationUnit.AcceptChildren(this, data);

            return null;
        }

        public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
        {
            Contract.Requires(typeDeclaration != null);

            // Is this a class but not a test fixture?
            if (IsClass(typeDeclaration) && !HasTestFixtureAttribute(typeDeclaration))
            {
                var parent = new JsClass()
                {
                    Name = typeDeclaration.Name,
                    Properties = new List<JsProperty>()
                };

                if (typeDeclaration.Attributes.Count > 0)
                {
                    foreach (var attribute in typeDeclaration.Attributes)
                    {
                        var jsAttributes = new List<JsAttribute>();
                        var a = attribute.Attributes[0];

                        jsAttributes.Add(new JsAttribute()
                        {
                            Name = a.Name
                        });

                        parent.Attributes = jsAttributes;
                    }
                }

                if (Model == null) Model = new JsFile();

                if (Model.Files == null) Model.Files = new List<JsClass>();

                Model.Files.Add(parent);

                CurrentParent = parent;
            }

            // Visit children (E.g. MethodDeclarion objects)
            typeDeclaration.AcceptChildren(this, data);

            return null;
        }

        public override object VisitBaseReferenceExpression(BaseReferenceExpression baseReferenceExpression, object data)
        {
            Contract.Requires(baseReferenceExpression != null);

            //Base class referenceExpression
            var foo = baseReferenceExpression;


            return null;
        }

        public override object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, object data)
        {
            Contract.Requires(propertyDeclaration != null);
            propertyDeclaration.AcceptChildren(this, data);

            if (propertyDeclaration.Modifier != Modifiers.Private)
            {
                var jsProperty = new JsProperty()
                {
                    Name = propertyDeclaration.Name,
                    IsArray = (propertyDeclaration.TypeReference.GenericTypes.Count > 0 || propertyDeclaration.TypeReference.IsArrayType)
                };

                CurrentParent.Properties.Add(jsProperty);
                if (propertyDeclaration.Attributes.Count > 0)
                {
                    var jsAttributes = new List<JsAttribute>();

                    foreach (var attribute in propertyDeclaration.Attributes)
                    {
                        var a = attribute.Attributes[0];

                        jsAttributes.Add(new JsAttribute()
                        {
                            Name = a.Name
                        });
                    }

                    jsProperty.Attributes = jsAttributes;
                }
            }

            return null;
        }

        public override object VisitFieldDeclaration(FieldDeclaration fieldsDeclaration, object data)
        {
            Contract.Requires(fieldsDeclaration != null);
            fieldsDeclaration.AcceptChildren(this, data);

            if (fieldsDeclaration.Attributes.Any(s => s.Attributes.Any(a => a.Name == "DataMember")))
            {
                foreach (var fieldDeclaration in fieldsDeclaration.Fields)
                {
                    var jsProperty = new JsProperty()
                    {
                        OfType = fieldsDeclaration.TypeReference.GenericTypes.Count == 0 ? fieldsDeclaration.TypeReference.Type : null,
                        Name = fieldDeclaration.Name,
                        IsArray = (fieldDeclaration.TypeReference.GenericTypes.Count > 0 || 
                            fieldsDeclaration.TypeReference.IsArrayType)
                    };

                    CurrentParent.Properties.Add(jsProperty);

                    // I can't figure out what this part is for, but I'm preserving it anyway
                    if (fieldsDeclaration.Attributes.Count > 0)
                    {
                        var jsAttributes = new List<JsAttribute>();

                        foreach (var attribute in fieldsDeclaration.Attributes)
                        {
                            var a = attribute.Attributes[0];

                            jsAttributes.Add(new JsAttribute()
                            {
                                Name = a.Name
                            });
                        }

                        jsProperty.Attributes = jsAttributes;
                    }
                }
            }

            return null;
        }

        public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
        {
            Contract.Requires(methodDeclaration != null);

            // Visit the body block statement of method declaration
            methodDeclaration.Body.AcceptVisitor(this, null);
            return null;
        }

        public override object VisitBlockStatement(BlockStatement blockStatement, object data)
        {
            Contract.Requires(blockStatement != null);

            // Visit children of block statement (E.g. several ExpressionStatement objects)
            blockStatement.AcceptChildren(this, data);

            return null;
        }

        public override object VisitExpressionStatement(ExpressionStatement expressionStatement, object data)
        {
            Contract.Requires(expressionStatement != null);

            // Visit the expression of the expression statement (E.g InnvocationExpression)
            expressionStatement.Expression.AcceptVisitor(this, null);

            return null;
        }

        public override object VisitInvocationExpression(InvocationExpression invocationExpression, object data)
        {
            Contract.Requires(invocationExpression != null);

            // Visit the target object of the invocation expression (E.g MemberReferenceExpression)
            invocationExpression.TargetObject.AcceptVisitor(this, null);
            return null;
        }

        public override object VisitMemberReferenceExpression(MemberReferenceExpression memberReferenceExpression, object data)
        {
            Contract.Requires(memberReferenceExpression != null);

            var identifierExpression = memberReferenceExpression.TargetObject as IdentifierExpression;

            // Is this a call to Contract.Requires(), Contract.Ensures() or Contract.Invariant()?
            if (identifierExpression != null &&
                identifierExpression.Identifier == "Contract" &&
                (memberReferenceExpression.MemberName == "Requires" ||
                 memberReferenceExpression.MemberName == "Ensures" ||
                 memberReferenceExpression.MemberName == "Invariant"))
            {
                //Assertion
            }

            return null;
        }

        #region private members

        static private bool IsClass(TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.Type == ClassType.Class;
        }

        static private bool HasTestFixtureAttribute(TypeDeclaration typeDeclaration)
        {
            bool hasTestFixtureAttribute = false;
            foreach (AttributeSection section in typeDeclaration.Attributes)
            {
                foreach (Attribute attribute in section.Attributes)
                {
                    if (attribute.Name == "TestFixture")
                    {
                        hasTestFixtureAttribute = true;
                        break;
                    }
                }
            }
            return hasTestFixtureAttribute;
        }

        #endregion
    }
}
