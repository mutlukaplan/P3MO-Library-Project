// /app/api/books/route.ts
import { NextRequest, NextResponse } from 'next/server';

const API_URL = process.env.API_URL || 'http://localhost:5078/api';

export async function GET(request: NextRequest) {
  try {
    const response = await fetch(`${API_URL}/books`);
    
    if (!response.ok) {
      throw new Error(`Error fetching books: ${response.statusText}`);
    }
    
    const data = await response.json();
    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in books API route:', error);
    return NextResponse.json(
      { error: 'Failed to fetch books' },
      { status: 500 }
    );
  }
}

export async function POST(request: NextRequest) {
    try {
      const body = await request.json();
      
      const response = await fetch(`${API_URL}/books`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)
      });
      
      if (!response.ok) {
        throw new Error(`Error creating book: ${response.statusText}`);
      }
      
      const data = await response.json();
      return NextResponse.json(data);
    } catch (error) {
      console.error('Error in books POST API route:', error);
      return NextResponse.json(
        { error: 'Failed to create book' },
        { status: 500 }
      );
    }
  }