import { redirect } from 'next/navigation';

export default function Home() {
  // This will redirect to the books page whenever the root page is accessed
  redirect('/books');
  
  // This return statement is technically unreachable but needed for TypeScript
  return null;
}